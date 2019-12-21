﻿using DatabaseLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLayer.Configurations
{
    public class TopicRatingConfiguration : IEntityTypeConfiguration<TopicRating>
    {
        public void Configure(EntityTypeBuilder<TopicRating> builder)
        {
            builder.Property(x => x.Rating).IsRequired();
            builder.HasKey(x => new { x.StudentId, x.TopicId });
        }
    }
}
