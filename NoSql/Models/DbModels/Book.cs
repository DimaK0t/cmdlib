﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoRepository;

namespace NoSql.Models.DbModels
{
    public class Book : Entity
    {
        public int BookNumber { get; set; }

        public string Author { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Extension { get; set; }
    }
}