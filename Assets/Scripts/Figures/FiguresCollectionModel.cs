using System;
using System.Collections.Generic;

public class FiguresCollectionModel: IEquatable<FiguresCollectionModel>
{
    public readonly FiguresTypes FiguresType;
    
    private List<FigureView> _figureViews;

    public List<FigureView> FigureViews => _figureViews;

    public FiguresCollectionModel(FiguresTypes type, int collectionLenth)
    {
        FiguresType = type;
        _figureViews = new List<FigureView>(collectionLenth);
    }

    public void AddView(FigureView view)
    {
        _figureViews.Add(view);
    }

    public int GetInBarCount()
    {
        var inBarCount = 0;

        for(int i = 0; i < _figureViews.Count; i++)
        {
            if (_figureViews[i].IsInBar)
                inBarCount++;
        }

        return inBarCount;
    }

    public void ClearCollection()
    {
        _figureViews.Clear();
    }

    public bool Equals(FiguresCollectionModel other)
    {
        if (other == null)
            return false;
        return this.FiguresType == other.FiguresType;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as FiguresCollectionModel);
    }

    public override int GetHashCode()
    {
        return FiguresType.GetHashCode();
    }
}
