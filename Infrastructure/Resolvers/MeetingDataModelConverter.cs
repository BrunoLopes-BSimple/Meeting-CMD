using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Resolvers
{
    public class MeetingDataModelConverter : ITypeConverter<MeetingDataModel, Meeting>
    {
        public Meeting Convert(MeetingDataModel source, Meeting destination, ResolutionContext context)
        {
            return new Meeting(source.Id, source.Period, source.Mode, source.LocationId);
        }
    }
}