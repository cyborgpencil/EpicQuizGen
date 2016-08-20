using EpicQuizGen.Models;
using Prism.Events;
using System.Collections.Generic;

namespace EpicQuizGen.Events
{
    public class SendQuestionNameEvent : PubSubEvent<string>{}
    public class SendMainQuestionEvent : PubSubEvent<string>{}
    public class SendQuestionTypesEvent : PubSubEvent<QuestionTypes>{}
    public class SendQuestionCategoryEvent : PubSubEvent<QuestionCategory>{}
    public class SendTrueFalseEvent : PubSubEvent<bool>{}
    public class SendMultiAnswerPositionsEvent : PubSubEvent<List<bool>>{ }
    public class SendMultiAnswerListEvent : PubSubEvent<List<string>> { }
    public class SendSelectedQuestionEvent : PubSubEvent<Question> { }
    public class SendQuestionEvent : PubSubEvent<Question> { }
    public class SendQuestionFromEditEvent : PubSubEvent<Question> { }
    public class SendMultiAnswer1Event : PubSubEvent<string> { }
    public class SendMultiAnswer2Event : PubSubEvent<string> { }
    public class SendMultiAnswer3Event : PubSubEvent<string> { }
    public class SendMultiAnswer4Event : PubSubEvent<string> { }
    public class TakeQuizEvent : PubSubEvent<Quiz> { }

}
