using System.ComponentModel.DataAnnotations;

namespace DesafioPedido.Tests
{
    public static class ValidationHelper
    {
        public static List<ValidationResult> ValidarTests(object model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(model, context, results, true);

            return results;
        }
    }
}
