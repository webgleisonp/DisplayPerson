using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web.Mvc;

namespace DisplayPerson.Core
{
    public class Common
    {
        public static T? GetValueOrNull<T>(string value) where T : struct
        {
            try
            {
                CultureInfo provider = new CultureInfo("pt-BR");

                if (!string.IsNullOrEmpty(value))
                    return (T)Convert.ChangeType(value, typeof(T), provider);

                return null;
            }
            catch (FormatException ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Não foi possível converter a string {0}, para o tipo {1}.", value, typeof(T).Name));
                sb.AppendLine(string.Format("Erro: {0}", ex.Message));

                throw new Exception(sb.ToString());
            }
        }

        public static string FormateDateString(string fileLine)
        {
            return string.Format("{0}/{1}/{2}", fileLine.Substring(46, 2).Trim(), fileLine.Substring(44, 2).Trim(), fileLine.Substring(40, 4).Trim());
        }

        public static IEnumerable<ModelValidationResult> Validate<TModel>(TModel model)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(r => r, new ViewDataDictionary<TModel>(model));

            ModelValidator validator = ModelValidator.GetModelValidator(modelMetadata, new ControllerContext());

            return validator.Validate(model);
        }
    }
}