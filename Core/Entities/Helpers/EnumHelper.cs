using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Core.Entities.Helpers
{
    public static class EnumHelper
    {
        public static List<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(x => new SelectListItem
                {
                    Value = x.ToString(),
                    Text = x.GetType()
                    .GetMember(x.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()?
                    .Name ?? x.ToString()
                }).ToList();
        }
    }
}
