using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System; 
using System.Windows.Input;

namespace Pjs1.Main.Helpers
{
    public class CommandParameterBindingConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var parameter in action.Parameters)
            {
                if (typeof(ICommand).IsAssignableFrom((parameter.ParameterInfo.ParameterType)))
                {
                    parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                    parameter.BindingInfo.BindingSource = BindingSource.Body;
                }
            }
        }
    }
}
