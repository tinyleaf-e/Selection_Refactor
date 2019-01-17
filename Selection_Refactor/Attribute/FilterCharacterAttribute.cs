using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Selection_Refactor.Attribute
{
    public class FilterCharacterAttribute : ValidationAttribute
    {
        //向构造函数中传递一个默认的错误提示消息，包含一个参数占位符{0}
        public FilterCharacterAttribute(string invalidCharacters) : base("{0}包含非法字符")
        {
            _invalidCharacters = invalidCharacters;
        }
        //第一个参数是验证对象的值
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string valueAsString = value.ToString();
                for(int i = 0; i < _invalidCharacters.Length; i++)
                {
                    if(valueAsString.Contains(_invalidCharacters[i]))
                    {
                        //FormatErrorMessage方法会自动使用显示的属性名称来格式化这个字符串
                        var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                        return new ValidationResult(errorMessage);
                    }
                }
            }
            return ValidationResult.Success;
        }
        private readonly string _invalidCharacters;
    }

    
}