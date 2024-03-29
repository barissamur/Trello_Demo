﻿namespace Tello_Demo.Web.Models;

public class CardList
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Title { get; set; } = null!;
    public string? Type { get; set; }
    public int Index { get; set; }

    public List<Card>? Cards { get; set; } = [];
}
