using System;

namespace Agile.Framework
{
    public class BusinessRule
    {
        public delegate bool BusinessRuleDelegate();

        /// <summary>
        /// ToString - overrided to show more useful when debugging
        /// </summary>
        public override string ToString()
        {
            return string.Format("[{0}] {1}", PropertyName, ErrorMessage);
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BusinessRule(string propertyName, string errorMessage, BusinessRuleDelegate ruleDelegate)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            RuleDelegate = ruleDelegate;
        }

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
        public BusinessRuleDelegate RuleDelegate { get; set; }

        public bool IsValid
        {
            get { return RuleDelegate(); }
        }
    }
}
