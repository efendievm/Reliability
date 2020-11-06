using System;
using System.Collections.Generic;

namespace Types
{   
    public enum SystemStructureElementType { GroupOfElements, ElementWithGivenDurability, ElementWithGivenSuccessProbability }
    public abstract class Element
    {
        public string Name { get; set; }
        public SystemStructureElementType Type {get; set; }
        public abstract Element Clone();
        public Element(string Name, SystemStructureElementType Type)
        {
            this.Name = Name;
            this.Type = Type;
        }
        public Element() { }
    }

    public abstract class ChildElement: Element
    {
        public double[] Cyclogramm { get; set; }
        public double CycleDuration;
        public ChildElement(string Name, SystemStructureElementType Type, double[] Cyclogramm, double CycleDuration)
            : base(Name, Type)
        {
            this.Cyclogramm = Cyclogramm;
            this.CycleDuration = CycleDuration;
        }
        public ChildElement() 
        {
            Cyclogramm = new double[0];
        }
    }
    
    public class GroupOfElements: Element
    {
        public double Majority { get; set; }
        public List<Element> ChildElements { get; set; }
        public override Element Clone()
        {
            var groupOfElements = new GroupOfElements();
            groupOfElements.Name = this.Name;
            groupOfElements.Type = this.Type;
            groupOfElements.Majority = this.Majority;
            foreach (var childElement in this.ChildElements)
                groupOfElements.ChildElements.Add(childElement.Clone());
            return groupOfElements;
        }
        public GroupOfElements(string Name, double Majority, List<Element> ChildElements)
            : base(Name, SystemStructureElementType.GroupOfElements)
        {
            this.Majority = Majority;
            this.ChildElements = ChildElements;
        }
        public GroupOfElements() 
        {
            ChildElements = new List<Element>();
            Type = SystemStructureElementType.GroupOfElements;
        }
    }
    
    public class ElementWithGivenDurability : ChildElement
    {
        public double Durability { get; set; }
        public override Element Clone()
        {
            return new ElementWithGivenDurability(this.Name, this.Cyclogramm, this.Durability, this.CycleDuration);
        }
        public ElementWithGivenDurability(string Name, double[] Cyclogramm, double Durability, double CycleDuration)
            : base(Name, SystemStructureElementType.ElementWithGivenDurability, Cyclogramm, CycleDuration)
        {
            this.Durability = Durability;
        }
        public ElementWithGivenDurability()
        {
            Type = SystemStructureElementType.ElementWithGivenDurability;
        }
    }

    public class ElementWithGivenSuccessProbability : ChildElement
    {
        public double SuccessProbability { get; set; }
        public override Element Clone()
        {
            return new ElementWithGivenSuccessProbability(this.Name, this.Cyclogramm, this.SuccessProbability, this.CycleDuration);
        }
        public ElementWithGivenSuccessProbability(string Name, double[] Cyclogramm, double SuccessProbability, double CycleDuration)
            : base(Name, SystemStructureElementType.ElementWithGivenSuccessProbability, Cyclogramm, CycleDuration)
        {
            this.SuccessProbability = SuccessProbability;
        }
        public ElementWithGivenSuccessProbability()
        {
            Type = SystemStructureElementType.ElementWithGivenSuccessProbability;
        }
    }
    
    public class SystemStructure
    {
        public List<Element> IndividualElements 
        { 
            get
            {
                var individualElements = new List<Element>();
                Action<Element> AddIndividualElemens = null;
                AddIndividualElemens = Element =>
                {
                    if (Element.Type != SystemStructureElementType.GroupOfElements)
                        individualElements.Add(Element);
                    else
                    {
                        var groupOfElements = Element as GroupOfElements;
                        for (int i = 0; i < groupOfElements.ChildElements.Count; i++)
                            AddIndividualElemens(groupOfElements.ChildElements[i]);
                    }
                };
                AddIndividualElemens(Root);
                return individualElements;
            }
        }
        public GroupOfElements Root { get; private set; }
        public SystemStructure()
        {
            Root = new GroupOfElements();
            Root.Name = "SystemStructure";
            Root.Majority = 0;
        }
        public SystemStructure Clone()
        {
            SystemStructure structure = new SystemStructure();
            structure.Root = (GroupOfElements)this.Root.Clone();
            return structure;
        }
        public SystemStructure(GroupOfElements Root)
        {
            this.Root = Root;
            this.Root.Name = "SystemStructure";
            this.Root.Majority = 0;
        }
    }

    public class InputData
    {
        public SystemStructure SystemStructure { get; private set; }
        public int SimulationCountForSucessProbabilityCalculation { get; private set; }
        public double TimeForSuccessProbabilityCalculation { get; private set; }
        public int GystogrammSimulationCount { get; private set; }
        public double GystogrammInitialTime { get; private set; }
        public InputData(
            SystemStructure systemStructure, 
            int simulationCountForSuccessProbabilityCalculation, 
            double timeForSucessProbabilityCalculation, 
            int gystogrammSimulationCount, 
            double gystogrammInitialTime)
        {
            SystemStructure = systemStructure;
            SimulationCountForSucessProbabilityCalculation = simulationCountForSuccessProbabilityCalculation;
            TimeForSuccessProbabilityCalculation = timeForSucessProbabilityCalculation;
            GystogrammSimulationCount = gystogrammSimulationCount;
            GystogrammInitialTime = gystogrammInitialTime;
        }
    }

    public enum SimulationResult { Success = 1, Failure = 0 }

    public class ReliabilitySimulation
    {
        public struct ReliabilityCurrentSimulation
        {
            public SimulationResult[] ElementsCurrentSimulationResult;
            public SimulationResult SystemCurrentSimulationResult;
        }
        public List<ReliabilityCurrentSimulation> SimulationResult {get; private set; }
        public ReliabilitySimulation(List<ReliabilityCurrentSimulation> SimulationsResult)
        {
            this.SimulationResult = SimulationsResult;
        }
    }

    public class ReliabilityReport
    {
        private int SimulationCount;
        public List<int> ElementsFailures {get; private set; }
        public List<int> SystemFailuresConcernedWithElements {get; private set; }
        public int SystemFailures { get; private set; }
        public double SystemSuccessPrabability { get { return 100.0 - SystemFailures * 100.0 / SimulationCount; } }
        public ReliabilityReport(int SimulationCount, List<int> ElementsFailures, List<int> SystemFailuresConcernedWithElements, int SystemFailures)
        {
            this.SimulationCount = SimulationCount;
            this.ElementsFailures = ElementsFailures;
            this.SystemFailuresConcernedWithElements = SystemFailuresConcernedWithElements;
            this.SystemFailures = SystemFailures;
        }
    }

    public class Gystogramm
    {
        public List<double> Times { get; private set; }
        public List<int> SystemFailures { get; private set; }
        public List<List<int>> ElementsFailures { get; private set; }
        public Gystogramm(List<double> Times, List<int> SystemFailures, List<List<int>> ElementsFailures)
        {
            this.Times = Times;
            this.SystemFailures = SystemFailures;
            this.ElementsFailures = ElementsFailures;
        }
    }
}