using System.Text;

namespace MateMachine.CurrencyConverter.Logic
{
    public class Step
    {
        public Step(Step previousStep, string from, string to, double rate, ExchageRateOperation operation, double previousStepRate)
        {
            StepNumber = previousStep == null ? 1 : previousStep.StepNumber + 1;
            From = from;
            To = to;
            Rate = rate;
            Operation = operation;
            PreviousStep = previousStep;
            PreviousStepRate = previousStepRate;
        }

        public int StepNumber { get; private set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Rate { get; set; }
        public ExchageRateOperation Operation { get; set; }
        public List<Step> NextSteps { get; set; }
        public Step PreviousStep { get; set; }
        public double PreviousStepRate { get; set; }

        public override string ToString()
        {
            StringBuilder stringbuilder = new();
            if (PreviousStep != null)
            {
                stringbuilder.Append(PreviousStep.ToString());
            }
            string sign = Operation == ExchageRateOperation.Multiply ? "*" : "/";
            stringbuilder.Append($"{StepNumber}: {From}-> {sign} ->{To}\n");
            return stringbuilder.ToString();
        }

        public ExchageRateOperation GetFirstStepOperatoin()
        {
            if (PreviousStep != null)
                return PreviousStep.GetFirstStepOperatoin();

            return Operation;
        }

        public string GetAllSteps()
        {
            string steps = "";
            if (PreviousStep != null)
                steps = PreviousStep.GetAllSteps();

            steps += Operation == ExchageRateOperation.Divide ? ",/," : ",*,";
            steps += Rate.ToString();
            return steps;
        }
    }
}
