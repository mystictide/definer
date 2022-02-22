﻿using definer.Entity.Helpers;
using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IDMessages : IBaseInterface<DMessages>
    {
        FilteredList<DMessages> FilteredList(FilteredList<DMessages> request, int UserID);
    }
}