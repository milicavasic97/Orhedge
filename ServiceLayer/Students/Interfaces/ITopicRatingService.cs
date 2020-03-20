﻿using ServiceLayer.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Students.Interfaces
{
    public interface ITopicRatingService : ICRUDServiceTemplate<TopicRatingDTO>, ISelectableServiceTemplate<TopicRatingDTO>
    {
    }
}
