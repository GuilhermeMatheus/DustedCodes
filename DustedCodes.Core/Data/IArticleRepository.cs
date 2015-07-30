﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace DustedCodes.Core.Data
{
    public interface IArticleRepository
    {
        Task<Article> GetAsync(string id);

        Task<ICollection<Article>> GetAllOrderedByDateAsync();
    }
}