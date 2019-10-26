using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lottery<T> where T : class {
    
    private List<Ticket<T>> tickets = new List<Ticket<T>>();
    private float weightCount = 0;

    public void Add(T item, float weight)
    {
        tickets.Add(new Ticket<T>(item, weight));
        weightCount += weight;
    }

    public void Remove(Ticket<T> ticket)
    {
        weightCount -= ticket.Weight;
        tickets.Remove(ticket);
    }

    public void Remove(T platform)
    {
        var ti = tickets.FindAll(t => t.Item == platform).FirstOrDefault();
        if (ti == null)
            return;
        Remove(ti);
    }

    public void SetList(List<Ticket<T>> tickets)
    {
        this.tickets = tickets;
        weightCount = tickets.Sum(t => t.Weight);
    }

    public T Draw()
    {
        float r = Random.Range(0f, weightCount);
        float min = 0;
        float max = 0;
        Ticket<T> winner = null;
        foreach (var ticket in tickets)
        {
            max += ticket.Weight;

            if (min <= r && r < max)
            {
                winner = ticket;
                break;
            }

            min = max;
        }

        return winner.Item;
    }
}

[System.Serializable]
public class Ticket<T> where T : class
{
    public T Item;
    public float Weight;

    public Ticket(T item, float weight)
    {
        Item = item;
        Weight = weight;
    }
}
