﻿using EpicQuizGen.Models;
using Prism.Events;
using System.Collections.Generic;

namespace EpicQuizGen.Events
{
    public class SendQuestionNameEvent : PubSubEvent<string>{}
    public class SendMainQuestionEvent : PubSubEvent<string>{}
    public class SendQuestionTypesEvent : PubSubEvent<QuestionTypes>{}
    public class SendCategoryEvent : PubSubEvent<QuestionCategory>{}
    public class SendTrueFalseEvent : PubSubEvent<bool>{}
    public class SendMultiAnswerPositionsEvent : PubSubEvent<List<bool>>{ }
    public class SendMultiAnswerListEvent : PubSubEvent<List<string>> { }
}
