﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VokimiShared.src.models.db_classes.test_answers
{
    public abstract class BaseAnswer
    {
        public AnswerId AnswerId { get; set; }
        public int Points { get; init; }
    }
}
