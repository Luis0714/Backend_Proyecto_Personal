﻿using Ardalis.Specification;
using Domain.Entities;
namespace Application.Espesification
{
    public class GetUserByEmail : Specification<User>
    {
        public GetUserByEmail(string Email)
        {
            Query.Where(user => user.Email == Email);
        }
    }
}
