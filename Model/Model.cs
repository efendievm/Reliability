using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ModelInterface;
using Types;
using System.IO;
using System.Threading;

namespace Model
{
    static class StaticRandom
    {
        static int seed = Environment.TickCount;
        static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));
        public static double NextDouble()
        {
            return random.Value.NextDouble();
        }
    }
    abstract class TimeInterceptor
    {
        public abstract double Intercept(double StartTime, double EndTime);
    }
    class TimeInterceptionForElementWithGivenDurability : TimeInterceptor
    {
        struct TimeInterval
        {
            public double StartTime;
            public double EndTime;
            public TimeInterval(double StartTime, double EndTime)
            {
                this.StartTime = StartTime;
                this.EndTime = EndTime;
            }
        }
        TimeInterval[] TimeIntervals;
        public override double Intercept(double StartTime, double EndTime)
        {
            double InterceptedTime = 0;
            foreach (var timeInterval in TimeIntervals)
            {
                if ((timeInterval.StartTime >= StartTime) && (timeInterval.StartTime <= EndTime))
                    InterceptedTime += Math.Min(timeInterval.EndTime, EndTime) - timeInterval.StartTime;
                else if ((timeInterval.EndTime >= StartTime) && (timeInterval.EndTime <= EndTime))
                    InterceptedTime += timeInterval.EndTime - Math.Max(timeInterval.StartTime, StartTime);
                else if ((timeInterval.StartTime <= StartTime) && (timeInterval.EndTime >= EndTime))
                    InterceptedTime = EndTime - StartTime;
            }
            return InterceptedTime;
        }
        public TimeInterceptionForElementWithGivenDurability(double[] Cyclogramm)
        {
            TimeIntervals = new TimeInterval[Cyclogramm.Length / 2];
            for (int i = 0; i < TimeIntervals.Length; i++)
                TimeIntervals[i] = new TimeInterval(Cyclogramm[2 * i], Cyclogramm[2 * i + 1]);
        }
    }
    class TimeInterceptionForElementWithGivenSuccessProbability : TimeInterceptor
    {
        double[] Cyclogramm;
        public override double Intercept(double StartTime, double EndTime)
        {
            double InterceptedTime = 0;
            foreach (var Time in Cyclogramm)
            {
                if ((Time >= StartTime) && (Time <= EndTime))
                    InterceptedTime += 1;
            }
            return InterceptedTime;
        }
        public TimeInterceptionForElementWithGivenSuccessProbability(double[] Cyclogramm)
        {
            this.Cyclogramm = Cyclogramm;
        }
    }
    abstract class ChildElementSimulation
    {
        protected double CycleDuration;
        protected double WithinCycleWorkingTime;
        protected TimeInterceptor timeInterceptor;
        public abstract SimulationResult Simulate(double Time, double TimeStep);
        protected ChildElementSimulation(TimeInterceptor timeInterceptor, double CycleDuration)
        {
            this.timeInterceptor = timeInterceptor;
            this.CycleDuration = CycleDuration;
            WithinCycleWorkingTime = timeInterceptor.Intercept(0, CycleDuration);
        }
    }
    class ElementWithGivenDurabilitySimulation : ChildElementSimulation
    {
        double Durability;
        public override SimulationResult Simulate(double Time, double TimeStep)
        {
            double WorkTime = 0;
            if (Math.Truncate(TimeStep / CycleDuration) >= 1)
            {
                int CycleCount = (int)Math.Truncate(Time / CycleDuration);
                WorkTime = CycleCount * WithinCycleWorkingTime + timeInterceptor.Intercept(0, Time - CycleCount * CycleDuration);
            }
            else
            {
                double PreviousTime = Time - TimeStep;
                int CycleCount = (int)Math.Truncate(PreviousTime / CycleDuration);
                if (Time - CycleCount * CycleDuration > CycleDuration)
                {
                    double workingTime =
                        timeInterceptor.Intercept(PreviousTime - CycleCount * CycleDuration, CycleDuration) +
                        timeInterceptor.Intercept(0, Time - (CycleCount + 1) * CycleDuration);
                    if (workingTime == 0)
                        return 0;
                    else
                        WorkTime = CycleCount * WithinCycleWorkingTime + timeInterceptor.Intercept(0, PreviousTime - CycleCount * CycleDuration) + workingTime;
                }
                else
                {
                    double workingTime = timeInterceptor.Intercept(PreviousTime - CycleCount * CycleDuration, Time - CycleCount * CycleDuration);
                    if (workingTime == 0)
                        WorkTime = 0;
                    else
                        WorkTime = CycleCount * WithinCycleWorkingTime + timeInterceptor.Intercept(0, PreviousTime - CycleCount * CycleDuration) + workingTime;
                }
            }
            return (StaticRandom.NextDouble() <= Math.Exp(-WorkTime * Durability * TimeStep)) ? SimulationResult.Success : SimulationResult.Failure;
        }
        public ElementWithGivenDurabilitySimulation(ElementWithGivenDurability element) 
            : base(new TimeInterceptionForElementWithGivenDurability(element.Cyclogramm), element.CycleDuration)
        {
            Durability = element.Durability;
        }
    }
    class ElementWithGivenSuccessProbabilitySimulation : ChildElementSimulation
    {
        double SuccessProbability;
        public override SimulationResult Simulate(double Time, double TimeStep)
        {
            int WorkCount = 0;
            if (Math.Truncate(TimeStep / CycleDuration) >= 1)
            {
                int CycleCount = (int)Math.Truncate(Time / CycleDuration);
                WorkCount = (int)timeInterceptor.Intercept(0, Time - CycleCount * CycleDuration);
            }
            else
            {
                double PreviousTime = Time - TimeStep;
                int CycleCount = (int)Math.Truncate(PreviousTime / CycleDuration);
                if (Time - CycleCount * CycleDuration > CycleDuration)
                {
                    double workingTime =
                        timeInterceptor.Intercept(PreviousTime - CycleCount * CycleDuration, CycleDuration) +
                        timeInterceptor.Intercept(0, Time - (CycleCount + 1) * CycleDuration);
                    if (workingTime == 0)
                        return 0;
                    else
                        WorkCount = (int)(timeInterceptor.Intercept(0, PreviousTime - CycleCount * CycleDuration) + workingTime);
                }
                else
                {
                    double workingTime = timeInterceptor.Intercept(PreviousTime - CycleCount * CycleDuration, Time - CycleCount * CycleDuration);
                    if (workingTime == 0)
                        WorkCount = 0;
                    else
                        WorkCount = (int)(timeInterceptor.Intercept(0, PreviousTime - CycleCount * CycleDuration) + workingTime);
                }
            }
            for (int i = 0; i < WorkCount; i++)
                if (StaticRandom.NextDouble() > SuccessProbability) return SimulationResult.Failure;
            return SimulationResult.Success;
        }
        public ElementWithGivenSuccessProbabilitySimulation(ElementWithGivenSuccessProbability element)
            : base(new TimeInterceptionForElementWithGivenSuccessProbability(element.Cyclogramm), element.CycleDuration)
        {
            SuccessProbability = element.SuccessProbability;
        }
    }
    class ElementWithGivenDurabilityDecorator : ElementWithGivenDurability
    {
        ChildElementSimulation Simulation;
        public SimulationResult Simulate(double Time, double TimeStep)
        {
            return Simulation.Simulate(Time, TimeStep);
        }
        public ElementWithGivenDurabilityDecorator(ElementWithGivenDurability element) :
            base(element.Name, element.Cyclogramm, element.Durability, element.CycleDuration)
        {
            Simulation = new ElementWithGivenDurabilitySimulation(element);
        }
    }
    class ElementWithGivenSuccessProbabilityDecorator : ElementWithGivenSuccessProbability
    {
        ChildElementSimulation Simulation;
        public SimulationResult Simulate(double Time, double TimeStep)
        {
            return Simulation.Simulate(Time, TimeStep);
        }
        public ElementWithGivenSuccessProbabilityDecorator(ElementWithGivenSuccessProbability element) :
            base(element.Name, element.Cyclogramm, element.SuccessProbability, element.CycleDuration)
        {
            Simulation = new ElementWithGivenSuccessProbabilitySimulation(element);
        }
    }

    public class Model : IModel
    {
        SystemStructure structure;
        private ReliabilitySimulation.ReliabilityCurrentSimulation SimulateSystemCurrentReliability(double Time, double TimeStep)
        {
            Func<Element, List<SimulationResult>> GetResult = null;
            GetResult = element =>
            {
                List<SimulationResult> res = new List<SimulationResult>();
                if (element.Type != SystemStructureElementType.GroupOfElements)
                {
                    SimulationResult elementResult;
                    if (element.Type == SystemStructureElementType.ElementWithGivenDurability)
                        elementResult = (element as ElementWithGivenDurabilityDecorator).Simulate(Time, TimeStep);
                    else
                        elementResult = (element as ElementWithGivenSuccessProbabilityDecorator).Simulate(Time, TimeStep);
                    res.Add(elementResult);
                    res.Add(elementResult);
                }
                else
                {
                    int SucceedChildElementsCount = 0;
                    var groupOfElements = element as GroupOfElements;
                    for (int i = 0; i < groupOfElements.ChildElements.Count ; i++)
                    {
                        List<SimulationResult> currentRes = GetResult(groupOfElements.ChildElements[i]);
                        for (int j = 0; j < currentRes.Count - 1; j++)
                            res.Add(currentRes[j]);
                        SucceedChildElementsCount += (int)currentRes.Last();
                    }
                    if (groupOfElements.Majority == 0)
                        res.Add((SucceedChildElementsCount == groupOfElements.ChildElements.Count) ? SimulationResult.Success : SimulationResult.Failure);
                    else
                        res.Add((SucceedChildElementsCount >= groupOfElements.Majority) ? SimulationResult.Success : SimulationResult.Failure);
                }
                return res;
            };
            List<SimulationResult> result = GetResult(structure.Root);
            ReliabilitySimulation.ReliabilityCurrentSimulation Result;
            Result.ElementsCurrentSimulationResult = result.Take(result.Count - 1).Select(x => (SimulationResult)x).ToArray();
            Result.SystemCurrentSimulationResult = (SimulationResult)result.Last();
            return Result;
        }
        private ReliabilitySimulation SimulateSystemReliability(int SimulationCount, double Time, double TimeStep)
        {            
            var SimulationResult = new List<ReliabilitySimulation.ReliabilityCurrentSimulation>();
            double ProgressReportStep = SimulationCount / 100.0;
            int CurrentSimulation = 0;
            int ProgressReport = 0;
            for (int i = 0; i < SimulationCount; i++)
            {
                var simulateSystemReliability = SimulateSystemCurrentReliability(Time, TimeStep);
                if (simulateSystemReliability.SystemCurrentSimulationResult == Types.SimulationResult.Failure)
                    SimulationResult.Add(simulateSystemReliability);
                CurrentSimulation++;
                ProgressReport++;
                if (ProgressReport >= ProgressReportStep)
                {
                    SuccessCalculationProgressChanged(this, new SimulationProgressChangedEventArgs((int)((CurrentSimulation * 100.0) / SimulationCount)));
                    ProgressReport = 0;
                }
            }
            SuccessCalculationProgressChanged(this, new SimulationProgressChangedEventArgs(100));
            return new ReliabilitySimulation(SimulationResult);
            
        }
        private ReliabilityReport GetReliabilityReport(int SimulationCount, double Time, double TimeStep)
        {
            int ProgressReport = 0;
            int BaseTaskCount = 1000;
            int TaskCount = ((SimulationCount / BaseTaskCount == 0) ? 0 : BaseTaskCount) + ((SimulationCount % BaseTaskCount == 0) ? 0 : 1);
            Mutex mutex = new Mutex();
            Func<int, Task<object[]>> GetTask = simulationCount =>
                new Task<object[]>(() =>
                {
                    List<int> _ElementsFailures = new List<int>();
                    List<int> _SystemFailuresConcernedWithElements = new List<int>();
                    for (int i = 0; i < structure.IndividualElements.Count; i++)
                    {
                        _ElementsFailures.Add(0);
                        _SystemFailuresConcernedWithElements.Add(0);
                    }            
                    int _SystemFailures = 0;
                    for (int i = 0; i < simulationCount; i++)
                    {
                        var simulateSystemReliability = SimulateSystemCurrentReliability(Time, TimeStep);
                        bool SystemFailed = simulateSystemReliability.SystemCurrentSimulationResult == Types.SimulationResult.Failure;
                        for (int j = 0; j < structure.IndividualElements.Count; j++)
                        {
                            if (simulateSystemReliability.ElementsCurrentSimulationResult[j] == SimulationResult.Failure)
                            {
                                _ElementsFailures[j]++;
                                if (SystemFailed)
                                    _SystemFailuresConcernedWithElements[j]++;
                            }
                        }
                        if (SystemFailed)
                            _SystemFailures++;
                    }
                    mutex.WaitOne();
                    ProgressReport++;
                    SuccessCalculationProgressChanged(this, new SimulationProgressChangedEventArgs((int)((ProgressReport * 100.0) / TaskCount)));
                    mutex.ReleaseMutex();
                    return new object[3] { _ElementsFailures, _SystemFailuresConcernedWithElements, _SystemFailures };
                });
            var TasksList = new List<Task<object[]>>();
            if (SimulationCount / BaseTaskCount != 0)
                for (int i = 0; i < BaseTaskCount; i++)
                    TasksList.Add(GetTask(SimulationCount / BaseTaskCount));
            if (SimulationCount % BaseTaskCount != 0)
                TasksList.Add(GetTask(SimulationCount % BaseTaskCount));
            foreach (var task in TasksList)
                task.Start();
            Task.WaitAll(TasksList.ToArray());
            List<object[]> TasksResults = TasksList.Select(x => x.Result).ToList();
            List<List<int>> ElementsFailuresFromTasks = TasksResults.Select(x => x[0] as List<int>).ToList();
            List<List<int>> SystemFailuresConcernedWithElementsFromTasks = TasksResults.Select(x => x[1] as List<int>).ToList();
            List<int> ElementsFailures = new List<int>();
            List<int> SystemFailuresConcernedWithElements = new List<int>();
            for (int elementIndex = 0; elementIndex < structure.IndividualElements.Count; elementIndex++)
            {
                int elementFailures = 0;
                int systemFailuresConcernedWithElement = 0;
                for (int taskIndex = 0; taskIndex < TaskCount; taskIndex++)
                {
                    elementFailures += ElementsFailuresFromTasks[taskIndex][elementIndex];
                    systemFailuresConcernedWithElement += SystemFailuresConcernedWithElementsFromTasks[taskIndex][elementIndex];
                }
                ElementsFailures.Add(elementFailures);
                SystemFailuresConcernedWithElements.Add(systemFailuresConcernedWithElement);
            }
            int SystemFailures = TasksResults.Select(x => (int)x[2]).Sum();
            SuccessCalculationProgressChanged(this, new SimulationProgressChangedEventArgs(100));
            return new ReliabilityReport(SimulationCount, ElementsFailures, SystemFailuresConcernedWithElements, SystemFailures);
        }
        public ReliabilitySimulation SimulateSystemReliability(int SimulationCount, double Time)
        {
            return SimulateSystemReliability(SimulationCount, Time, Time);
        }
        public ReliabilitySimulation SimulateSystemCurrentReliability(int SimulationCount, double Time)
        {
            return SimulateSystemReliability(SimulationCount, Time, 1.0 / 3600);
        }
        public ReliabilityReport GetReliabilityReport(int SimulationCount, double Time)
        {
            return GetReliabilityReport(SimulationCount, Time, Time);
        }
        public ReliabilityReport GetCurrentReliabilityReport(int SimulationCount, double Time)
        {
            return GetReliabilityReport(SimulationCount, Time, 1.0 / 3600);
        }
        public Gystogramm GetGystogramm(int SimulationCount, double InitialTime)
        {
            List<double> Times = new List<double>();
            List<List<double>> ElementsTimes = new List<List<double>>();
            for (int i = 0; i < structure.IndividualElements.Count; i++)
                ElementsTimes.Add(new List<double>());
            Mutex mutex = new Mutex();
            int Progress = 0;
            List<Task> TaskList = new List<Task>();
            double timeStep = 1;
            for (int i = 0; i < SimulationCount; i++)
                TaskList.Add(Task.Factory.StartNew(() =>
                {
                    double FailureTime = InitialTime;
                    ReliabilitySimulation.ReliabilityCurrentSimulation simulateSystemReliability;
                    simulateSystemReliability.SystemCurrentSimulationResult = SimulationResult.Success;
                    do
                    {
                        FailureTime += timeStep;
                        simulateSystemReliability = SimulateSystemCurrentReliability(FailureTime, timeStep);
                        int j = 0; 
                        foreach (var elementSimulation in simulateSystemReliability.ElementsCurrentSimulationResult) 
                        {
                            if (elementSimulation == SimulationResult.Failure)
                            {
                                mutex.WaitOne();
                                ElementsTimes[j].Add(FailureTime);
                                mutex.ReleaseMutex();
                            }
                            j++; 
                        }
                    } while (simulateSystemReliability.SystemCurrentSimulationResult == SimulationResult.Success);
                    mutex.WaitOne();
                    Times.Add(FailureTime);
                    Progress++;
                    GystogrammCalculationProgressChanged(this, new SimulationProgressChangedEventArgs((int)((Progress * 100.0) / SimulationCount)));
                    mutex.ReleaseMutex();
                }));
            Task.WaitAll(TaskList.ToArray());
            Times.Sort();
            double StartTime = Times.First();
            double EndTime = Times.Last();
            timeStep = Math.Max((EndTime - StartTime) / 100, timeStep);
            foreach (var elementTimes in ElementsTimes)
                elementTimes.Sort();
            List<double> GystogrammTimes = new List<double>();
            List<int> SystemFailures = new List<int>();
            List<List<int>> ElementsFailures = new List<List<int>>();
            for (int i = 0; i < structure.IndividualElements.Count; i++)
                ElementsFailures.Add(new List<int>());
            double time = StartTime;
            int CurrentSystemFailureTimeIndex = 0;
            List<int> CurrentElementsFailureTimeIndex = new List<int>();
            foreach (var elementTime in ElementsTimes)
            {
                int currentElementFailureTimeIndex = 0;
                if (elementTime.Count != 0)
                    while ((currentElementFailureTimeIndex != elementTime.Count) && (elementTime[currentElementFailureTimeIndex] < StartTime))
                        currentElementFailureTimeIndex++;
                CurrentElementsFailureTimeIndex.Add(currentElementFailureTimeIndex);
            }
            while (time <= EndTime)
            {
                GystogrammTimes.Add(time);
                int systemFailures = 0;
                while ((CurrentSystemFailureTimeIndex != Times.Count) && (Times[CurrentSystemFailureTimeIndex] <= time + timeStep))
                {
                    CurrentSystemFailureTimeIndex++;
                    systemFailures++;
                }
                SystemFailures.Add(systemFailures);
                for (int i = 0; i < ElementsFailures.Count; i++)
                {
                    int elementFailures = 0;
                    if (ElementsTimes[i].Count != 0)
                        while ((CurrentElementsFailureTimeIndex[i] != ElementsTimes[i].Count) && (ElementsTimes[i][CurrentElementsFailureTimeIndex[i]] <= time + timeStep))
                        {
                            CurrentElementsFailureTimeIndex[i]++;
                            elementFailures++;
                        }
                    ElementsFailures[i].Add(elementFailures);
                }
                time += timeStep;
            }
            return new Gystogramm(GystogrammTimes, SystemFailures, ElementsFailures);
        }
        public void SetStructure(SystemStructure systemStructure)
        {
            structure = systemStructure.Clone();
            Action<Element, Element, int> ChangeType = null;
            ChangeType = (Element, Parent, Index) =>
            {
                if (Element.Type != SystemStructureElementType.GroupOfElements)
                {
                    if (Element.Type == SystemStructureElementType.ElementWithGivenDurability)
                        (Parent as GroupOfElements).ChildElements[Index] = new ElementWithGivenDurabilityDecorator(Element as ElementWithGivenDurability);
                    else
                        (Parent as GroupOfElements).ChildElements[Index] = new ElementWithGivenSuccessProbabilityDecorator(Element as ElementWithGivenSuccessProbability);
                }
                else
                {
                    var groupOfElements = Element as GroupOfElements;
                    for (int i = 0; i < groupOfElements.ChildElements.Count; i++)
                        ChangeType(groupOfElements.ChildElements[i], groupOfElements, i);
                }
            };
            ChangeType(structure.Root, null, 0);
        }
        private SystemStructure LoadSystemStructure(XmlNode RootNode)
        {
            Func<XmlNode, Element> CreateElement = null;
            CreateElement = Node =>
            {
                if ((Node != RootNode) && (Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name == "Majority").Count() == 0))
                {
                    if (Node.Attributes.Cast<XmlAttribute>().Where(x => x.Name == "SuccessProbability").Count() != 0)
                    {
                        double[] Cyclogramm = new double[Node.ChildNodes[0].ChildNodes.Count];
                        for (int i = 0; i < Cyclogramm.Length; i++)
                            Cyclogramm[i] = Convert.ToDouble(Node.ChildNodes[0].ChildNodes[i].Attributes[0].InnerText);
                        return new ElementWithGivenSuccessProbability(
                            Node.Attributes["Name"].InnerText,
                            Cyclogramm,
                            Convert.ToDouble(Node.Attributes["SuccessProbability"].InnerText),
                            Convert.ToDouble(Node.Attributes["CycleDuration"].InnerText));
                    }
                    else
                    {
                        double[] Cyclogramm = new double[2 * Node.ChildNodes[0].ChildNodes.Count];
                        for (int i = 0; i < Node.ChildNodes[0].ChildNodes.Count; i++)
                        {
                            Cyclogramm[2 * i] = Convert.ToDouble(Node.ChildNodes[0].ChildNodes[i].Attributes[0].InnerText);
                            Cyclogramm[2 * i + 1] = Convert.ToDouble(Node.ChildNodes[0].ChildNodes[i].Attributes[1].InnerText);
                        }
                        return new ElementWithGivenDurability(
                                Node.Attributes["Name"].InnerText,
                                Cyclogramm,
                                Convert.ToDouble(Node.Attributes["Durability"].InnerText),
                                Convert.ToDouble(Node.Attributes["CycleDuration"].InnerText));
                    }
                }
                else
                {
                    var Element = new GroupOfElements();
                    if (Node != RootNode)
                    {
                        Element.Name = Node.Attributes["Name"].InnerText;
                        Element.Majority = Convert.ToDouble(Node.Attributes["Majority"].InnerText);
                    }
                    else
                    {
                        Element.Name = Node.Name;
                        Element.Majority = 0;
                    }
                    for (int i = 0; i < Node.ChildNodes.Count; i++)
                        Element.ChildElements.Add(CreateElement(Node.ChildNodes[i]));
                    return Element;
                }
            };
            return new SystemStructure((GroupOfElements)CreateElement(RootNode));
        }
        public SystemStructure LoadSystemStructure(string FileName)
        {
            using (StreamReader sr = new StreamReader(FileName, Encoding.UTF8))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sr);
                return LoadSystemStructure(xmlDoc.ChildNodes[0]);
            }
        }
        public InputData LoadInputData(XmlDocument xmlDoc)
        {
            Func<string, XmlNode> GetNode = Name => xmlDoc.ChildNodes[0].ChildNodes.Cast<XmlNode>().First(x => x.Name == Name);
            var n1 = GetNode("SimulationCountForSuccessProbabilityCalculation");
            var i1 = Convert.ToInt32(n1.InnerText);
            return new InputData(
                LoadSystemStructure(GetNode("SystemStructure")),
                Convert.ToInt32(GetNode("SimulationCountForSuccessProbabilityCalculation").InnerText),
                Convert.ToDouble(GetNode("TimeForSuccessProbabilityCalculation").InnerText),
                Convert.ToInt32(GetNode("GystogrammSimulationCount").InnerText),
                Convert.ToDouble(GetNode("GystogrammInitialTime").InnerText));
        }
        public InputData LoadInputData(string FileName)
        {
            using (StreamReader sr = new StreamReader(FileName, Encoding.UTF8))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(sr);
                return LoadInputData(xmlDoc);
            }
        }
        private XmlDocument GetXml(SystemStructure systemStructure)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode RootNode = xmlDoc.CreateElement("SystemStructure");
            Action<XmlNode, Element> AddNode = null;
            AddNode = (Node, Element) =>
            {
                XmlNode node = xmlDoc.CreateElement("Element");
                Action<XmlNode, string, string> addAttribute = (AddToNode, AttributeName, AttributeValue) =>
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(AttributeName);
                    attribute.InnerText = AttributeValue;
                    AddToNode.Attributes.Append(attribute);
                };
                addAttribute(node, "Name", Element.Name);
                if (Element.Type != SystemStructureElementType.GroupOfElements)
                {
                    if (Element.Type == SystemStructureElementType.ElementWithGivenSuccessProbability)
                    {
                        var element = Element as ElementWithGivenSuccessProbability;
                        addAttribute(node, "SuccessProbability", element.SuccessProbability.ToString());
                        addAttribute(node, "CycleDuration", element.CycleDuration.ToString());
                        XmlNode cyclogrammNode = xmlDoc.CreateElement("Cyclogramm");
                        for (int i = 0; i < element.Cyclogramm.Length; i++)
                        {
                            XmlNode stateNode = xmlDoc.CreateElement("WorksAt");
                            addAttribute(stateNode, "Time", element.Cyclogramm[i].ToString());
                            cyclogrammNode.AppendChild(stateNode);
                        }
                        node.AppendChild(cyclogrammNode);
                    }
                    else
                    {
                        var element = Element as ElementWithGivenDurability;
                        addAttribute(node, "Durability", element.Durability.ToString());
                        addAttribute(node, "CycleDuration", element.CycleDuration.ToString());
                        XmlNode cyclogrammNode = xmlDoc.CreateElement("Cyclogramm");
                        for (int i = 0; i < element.Cyclogramm.Length / 2; i++)
                        {
                            XmlNode stateNode = xmlDoc.CreateElement("WorksInInterval");
                            addAttribute(stateNode, "StartTime", element.Cyclogramm[2 * i].ToString());
                            addAttribute(stateNode, "EndTime", element.Cyclogramm[2 * i + 1].ToString());
                            cyclogrammNode.AppendChild(stateNode);
                        }
                        node.AppendChild(cyclogrammNode);
                    }
                }
                else
                {
                    var element = Element as GroupOfElements;
                    addAttribute(node, "Majority", element.Majority.ToString());
                    for (int i = 0; i < element.ChildElements.Count; i++)
                        AddNode(node, element.ChildElements[i]);
                }
                Node.AppendChild(node);
            };
            AddNode(RootNode, systemStructure.Root);
            XmlNode rootNode = xmlDoc.CreateElement(RootNode.ChildNodes[0].Attributes["Name"].InnerText);
            for (int i = 0; i < RootNode.ChildNodes[0].ChildNodes.Count; i++)
                rootNode.AppendChild(RootNode.ChildNodes[0].ChildNodes[i].Clone());
            xmlDoc.AppendChild(rootNode);
            return xmlDoc;
        }
        public void SaveInputData(string FileName, InputData inputData)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode RootNode = xmlDoc.CreateElement("InputData");
            RootNode.AppendChild(xmlDoc.ImportNode(GetXml(inputData.SystemStructure).ChildNodes[0], true));
            Action<string, double> AddNode = (Name, Value) =>
            {
                var node = xmlDoc.CreateElement(Name);
                node.InnerText = Value.ToString();
                RootNode.AppendChild(node);
            };
            AddNode("SimulationCountForSuccessProbabilityCalculation", inputData.SimulationCountForSucessProbabilityCalculation);
            AddNode("TimeForSuccessProbabilityCalculation", inputData.TimeForSuccessProbabilityCalculation);
            AddNode("GystogrammSimulationCount", inputData.GystogrammSimulationCount);
            AddNode("GystogrammInitialTime", inputData.GystogrammInitialTime);
            xmlDoc.AppendChild(RootNode);
            xmlDoc.Save(FileName);
        }
        public StringBuilder GetSystemXmlOutlet(SystemStructure systemStructure)
        {
            var tw = new System.IO.StringWriter();
            GetXml(systemStructure).Save(tw);
            return tw.GetStringBuilder();
        }
        public event EventHandler<SimulationProgressChangedEventArgs> SuccessCalculationProgressChanged;
        public event EventHandler<SimulationProgressChangedEventArgs> GystogrammCalculationProgressChanged;
    }
}