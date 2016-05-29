// ============================================================================
// FileName			:	BaseBiz.cs
// Summary			:	Base class for business objects
// ============================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Agile.Shared;

namespace Agile.Framework
{
    public interface IBaseBiz : INotifyPropertyChanged
    {
        bool IsNew { get; set; }
        bool IsValid();
        bool IsDirtyThis();
        string GetValidationMessages();
        void FillShallow<D>(D data) where D : IModelInterface;
        long GetId();
//        Guid AltId { get; set; }
        void SetId(long id);

    }

    /// <summary>
    /// Base class for business objects
    /// </summary>
    public abstract class BaseBiz : IBaseBiz
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BaseBiz()
        {
            IsNew = true; // set to true by default
            AltId = Guid.NewGuid(); // set by default
        }

        public override string ToString()
        {
            return string.Format("[{0}]", GetId());
        }

        private Guid altIdField;
        /// <summary>
        /// Gets and sets AltId.    
        /// </summary>
        public Guid AltId
        {
            get { return altIdField; }
            set
            {
                if (altIdField == value)
                    return;
                altIdField = value;

                RaisePropertyChanged("AltId");
            }
        }

        // default to Now, primarily helps client side syncronization (CacheItem), server should always override this default value to the value from the db.
        protected DateTimeOffset createdField = AgileDateTime.UtcNow;

        /// <summary>
        /// Gets and sets Created.    
        /// </summary>
        public DateTimeOffset Created
        {
            get { return createdField; }
            set
            {
                if (createdField == value)
                    return;
                createdField = value;

                RaisePropertyChanged("Created");
            }
        }

        public abstract long GetId();
        public abstract void SetId(long id);

        /// <summary>
        /// Gets and sets if the item is new. If null then it has not been set.
        /// </summary>
        public bool IsNew { get; set; }

        public virtual void FillShallow<D>(D data) where D : IModelInterface
        {
        }

        // note: Clone extension methods are in the Helper class.

        /// <summary>
        /// Event handler for when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler PrimaryKeyPropertyChanged;
        /// <summary>
        /// Raise the property changed event for the given property
        /// </summary>
        public void RaisePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (IsPrimaryKeyField(propertyName) && PrimaryKeyPropertyChanged != null)
                PrimaryKeyPropertyChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// work in progress
        /// </summary>
        protected virtual bool IsPrimaryKeyField(string name)
        {
            return false;
        }

        #region Validation

        private List<BusinessRule> ruleCollection;
        /// <summary>
        /// Gets all of the rules that have been defined for the biz object
        /// </summary>
        protected List<BusinessRule> RuleCollection
        {
            get
            {
                if (ruleCollection == null)
                {
                    ruleCollection = new List<BusinessRule>();
                    DefineRules();
                }
                return ruleCollection;
            }
        }

        private bool validationEnabled = true; // enabled by default

        /// <summary>
        /// Enables all validation rules for the biz object (enabled by default)
        /// </summary>
        public void EnableValidation()
        {
            validationEnabled = true;
        }

        /// <summary>
        /// Disables all validation rules for the biz object (enabled by default)
        /// </summary>
        public void DisableValidation()
        {
            validationEnabled = false;
        }

        /// <summary>
        /// Must be implemented by all biz objects, inside the implementation the method should add
        /// all rules required for the biz object.
        /// </summary>
        /// <remarks>make sure you call base.DefineRules in any overrides!</remarks>
        protected virtual void DefineRules()
        {
            InternalDefineRules();
        }

        /// <summary>
        /// Generated rules.
        /// </summary>
        /// <remarks>TODO: investigate, should these rules be checked in the Service layer? as
        /// they are mainly data specific type rules like 'length is less than column.length'</remarks>
        protected virtual void InternalDefineRules()
        {
        }

        public abstract bool IsDirtyThis();

        public virtual bool IsValid()
        {
            return GetFailedBusinessRules().Count == 0;
        }

        public virtual List<BusinessRule> GetFailedBusinessRules()
        {
            var failedRules = RuleCollection.Where(rule => !rule.IsValid).ToList();

            return failedRules;
        }

        /// <summary>
        ///  Gets the Failed Business Rules as a string
        /// </summary>
        public string GetValidationMessages()
        {
            if (IsValid()) return string.Empty;
            var message = new StringBuilder();
            foreach (BusinessRule businessRule in GetFailedBusinessRules())
                message.AppendLine(businessRule.ErrorMessage);
            return message.ToString();
        }

        public virtual void ValidatePropertyValue(string propertyName)
        {
            if (validationEnabled)
            {
                foreach (BusinessRule rule in RuleCollection)
                {
                    if (rule.PropertyName == propertyName)
                    {
                        // Validate the rule.  Note that I don't break the loop afterwards - there may be
                        // more than one rule per property so we need to continue to look for more rules
                        if (!rule.IsValid)
                            throw new Exception(rule.ErrorMessage);
                    }
                }
            }
        }

        #endregion



        /// <summary>
        /// Validate the object
        /// </summary>
        public virtual bool Validate()
        {
            return true;
        }

    }
}