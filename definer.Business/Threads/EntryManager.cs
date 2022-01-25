﻿using definer.Core.Interface;
using definer.Core.Repo;
using definer.Entity.Helpers;
using definer.Entity.Threads;

namespace definer.Business.Threads
{
    public class EntryManager : IEntry
    {
        private readonly IEntry _repo;
        public EntryManager()
        {
            _repo = new EntryRepository();
        }

        public ProcessResult Add(Entry entity)
        {
            return _repo.Add(entity);
        }

        public ProcessResult Delete(int ID)
        {
            return _repo.Delete(ID);
        }

        public FilteredList<Entry> FilteredList(FilteredList<Entry> request)
        {
            return _repo.FilteredList(request);
        }

        public Entry Get(int ID)
        {
            return _repo.Get(ID);
        }

        public IEnumerable<Entry> GetAll()
        {
            return _repo.GetAll();
        }

        public ProcessResult Update(Entry entity)
        {
            return _repo.Update(entity);
        }
    }
}
