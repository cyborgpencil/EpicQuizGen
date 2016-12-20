using Prism.Mvvm;
using System.Collections.Generic;

namespace EpicQuizGen.Models
{
    public enum QuestionCategory
    {
        OPHTHALMIC,
        INTFORMATION_TECHNOLOGY,
        MISC
    }
    public class QuestionCategories : BindableBase
    {
        public List<string> CurrentCategoryList;
        
    }
}
