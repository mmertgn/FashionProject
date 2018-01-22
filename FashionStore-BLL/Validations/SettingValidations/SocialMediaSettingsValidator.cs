using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FashionStore.Entity.Entities;
using FluentValidation;

namespace FashionStore_BLL.Validations.SettingValidations
{
    public class SocialMediaSettingsValidator : AbstractValidator<Setting>
    {
        public SocialMediaSettingsValidator()
        {

        }
    }
}
