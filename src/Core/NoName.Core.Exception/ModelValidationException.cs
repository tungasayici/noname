using System;
using System.Collections.Generic;
using System.Net;

namespace NoName.Core.Exception
{
    public class ModelValidationException : BaseException
    {
        public List<ModelValidation> _modelValidationList { get; set; }

        public ModelValidationException(string propName, string validationErrorDescription) : base(HttpStatusCode.BadRequest)
        {
            _modelValidationList = new List<ModelValidation>();
            _modelValidationList.Add(new ModelValidation { PropName = propName, ValidationErrorDescription = validationErrorDescription });
        }

        public ModelValidationException(List<ModelValidation> modelValidationList) : base(HttpStatusCode.BadRequest)
        {
            _modelValidationList = modelValidationList ?? throw new ArgumentNullException($"{nameof(modelValidationList)} is null!");
        }

        public override object GenerateExceptionModel()
        {
            var errorModel = new List<object>();
            _modelValidationList.ForEach((item) => errorModel.Add(new { PropertyName = item.PropName, ValidationErrorDescription = item.ValidationErrorDescription }));
            return errorModel;
        }
    }

    public class ModelValidation
    {
        public string PropName { get; set; }
        public string ValidationErrorDescription { get; set; }
    }
}