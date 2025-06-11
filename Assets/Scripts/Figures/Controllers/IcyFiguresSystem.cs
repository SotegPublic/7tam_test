using System.Collections.Generic;

public class IcyFiguresSystem : IIcyFiguresSystem, IChangableIcyFiguresSystem
{
    private FigureModifiersConfig _modifiersConfig;

    private List<FigureView> _icyFigures;
    private int _currentDeletedFigures;

    public int CurrentDeletedFigures => _currentDeletedFigures;

    public IcyFiguresSystem(FigureModifiersConfig modifiersConfig)
    {
        _modifiersConfig = modifiersConfig;
        _icyFigures = new List<FigureView>(_modifiersConfig.MaxCountIcyFigures);
    }

    public void AddIcyFigure(FigureView icyView)
    {
        _icyFigures.Add(icyView);
    }

    public void TryCrackIce()
    {
        _currentDeletedFigures++;

        if (_currentDeletedFigures >= _modifiersConfig.FiguresCountDeleteForIceBreak)
        {
            for(int i = 0; i < _icyFigures.Count; i++)
            {
                _icyFigures[i].CrackIce();
            }
        }
    }

    public void ClearCurrentCollection()
    {
        _icyFigures.Clear();
    }

    public void FullClear()
    {
        _icyFigures.Clear();
        _currentDeletedFigures = 0;
    }
}
