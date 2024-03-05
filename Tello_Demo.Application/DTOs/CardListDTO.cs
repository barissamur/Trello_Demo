using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tello_Demo.Domain.Models;

namespace Tello_Demo.Application.DTOs;

public class CardListDTO 
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Title { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int Index { get; set; }

    public List<CardDTO> Cards { get; set; } = [];
}
