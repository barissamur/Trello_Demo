using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tello_Demo.Application.Interfaces;

public interface IRepo<T> : IRepositoryBase<T> where T : class
{
}
