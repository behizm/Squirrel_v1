using System.Collections.Generic;
using System.Linq;

namespace Squirrel.Domain.ResultModels
{
    public class OperationResult
    {
        public OperationResult(params string[] errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        public OperationResult(IEnumerable<string> errors)
        {
            Succeeded = false;
            Errors = errors.ToArray();
        }

        protected OperationResult(bool succeeded)
        {
            Succeeded = succeeded;
            Errors = new string[] { };
        }


        public bool Succeeded { get; private set; }
        public string[] Errors { get; private set; }


        public static OperationResult Success
        {
            get
            {
                return new OperationResult(true);
            }
        }

        public static OperationResult Failed(params string[] errors)
        {
            return new OperationResult(errors);
        }
    } 
}