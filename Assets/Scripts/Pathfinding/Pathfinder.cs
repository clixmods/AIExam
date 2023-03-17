using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 

public static class Pathfinder
{
    #region Common & Utilities 
    private static Cell[] BuildPath(Cell _startCell, Cell _endCell, Dictionary<Cell, Cell> _cameFrom, Tilemap _map, Tile _pathTile)
    {
        List<Cell> _path = new List<Cell>{ _endCell};
        Cell _currentCell = _endCell;
        _map.SetTile(_currentCell.Position, _pathTile);
        while (_currentCell != _startCell)
        {
            _currentCell = _cameFrom[_currentCell]; 
            _path.Add(_currentCell);
            _map.SetTile(_currentCell.Position, _pathTile); 
        }
        _path.Reverse();

        return _path.ToArray(); 
    }

    private static Cell[] BuildPath(Cell _startCell, Cell _endCell, Dictionary<Cell, Cell> _cameFrom)
    {
        List<Cell> _path = new List<Cell> { _endCell };
        Cell _currentCell = _endCell;
        while (_currentCell != _startCell)
        {
            _currentCell = _cameFrom[_currentCell];
            _path.Add(_currentCell);
        }
        _path.Reverse();

        return _path.ToArray();
    }

    private static int GetBestCellIndex(Cell[] _frontier, Cell _endCell)
    {
        float _bestHeuristic = int.MaxValue;
        float _currentHeuristic = _bestHeuristic;
        int _bestIndex = -1;
        for (int i = 0; i < _frontier.Length; i++)
        {
            _currentHeuristic = _frontier[i].GetHeuristicCost(_endCell.Position);
            if (_currentHeuristic < _bestHeuristic)
            {
                _bestHeuristic = _currentHeuristic;
                _bestIndex = i;
            }
        }
        return _bestIndex;
    }
    #endregion 


    #region BFS
    public static Cell[] FindBFSPath(Cell _startCell, Cell _endCell, Cell[,] _cells)
    {
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();
        List<Cell> _frontier = new List<Cell>{ _startCell };
        Cell _currentCell = null;
        Cell _neighbourCell = null;
        int _currentStep = 0; 
        while (_frontier.Count > 0)
        {
            _currentCell = _frontier[0];

            if (_currentCell == _endCell)
            {
            }
            
            if (_currentCell.IndexX > 0 && 
                (_neighbourCell = _cells[_currentCell.IndexX - 1, _currentCell.IndexY]).Cost < int.MaxValue && 
                !_cameFrom.ContainsKey(_neighbourCell) )
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell); 
            }

            if (_currentCell.IndexX < _cells.GetLength(0) - 1  &&
                (_neighbourCell = _cells[_currentCell.IndexX + 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY - 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY < _cells.GetLength(1) -1  &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY + 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }
            _currentStep++;
            _frontier.RemoveAt(0); 
        }
        return new Cell[0];
    }

    public static IEnumerator DebugBreadthFirstSearch(Cell _startCell, Cell _endCell, Cell[,] _cells, Tilemap _map, Tile _frontierTile, Tile _cameFromTile, Tile _pathTile, int _step = -1)
    {
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();
        List<Cell> _frontier = new List<Cell> { _startCell };
        Cell _currentCell = null;
        Cell _neighbourCell = null;
        int _currentStep = 0;
        while (_frontier.Count > 0 && _currentStep != _step)
        {
            _currentCell = _frontier[0];
            _map.SetTile(_currentCell.Position, _frontierTile);
            yield return null; 

            if (_currentCell == _endCell)
            {
                BuildPath(_startCell, _endCell, _cameFrom, _map, _pathTile);
                yield break; 
            }

            if (_currentCell.IndexX > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX - 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexX < _cells.GetLength(0) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX + 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY - 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY < _cells.GetLength(1) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY + 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }
            _currentStep++;
            _frontier.RemoveAt(0);
        }
        BuildPath(_startCell, _currentCell, _cameFrom, _map, _pathTile);
        yield return null; 
    }
    #endregion

    #region Djikstra 
    public static Cell[] FindDjikstraPath(Cell _startCell, Cell _endCell, Cell[,] _cells)
    {
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();
        List<Cell> _frontier = new List<Cell> { _startCell };
        Cell _currentCell = null;
        Cell _neighbourCell = null;
        int _currentStep = 0, _bestIndex = 0;
        while (_frontier.Count > 0)
        {
            _bestIndex = GetBestCellIndex(_frontier.ToArray(), _endCell); 
            _currentCell = _frontier[_bestIndex];

            if (_currentCell == _endCell)
            {
                return BuildPath(_startCell, _endCell, _cameFrom);
            }

            if (_currentCell.IndexX > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX - 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexX < _cells.GetLength(0) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX + 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY - 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY < _cells.GetLength(1) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY + 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }
            _currentStep++;
            _frontier.RemoveAt(_bestIndex);
        }

        return new Cell[0];
    }

    public static IEnumerator DebugDjikstra(Cell _startCell, Cell _endCell, Cell[,] _cells, Tilemap _map, Tile _frontierTile, Tile _cameFromTile, Tile _pathTile, int _step = -1)
    {
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();
        List<Cell> _frontier = new List<Cell> { _startCell };
        Cell _currentCell = null;
        Cell _neighbourCell = null;
        int _currentStep = 0, _bestIndex = 0;
        while (_frontier.Count > 0 && _currentStep != _step)
        {
            _bestIndex = GetBestCellIndex(_frontier.ToArray(), _endCell); 
            _currentCell = _frontier[_bestIndex];
            _map.SetTile(_currentCell.Position, _frontierTile);
            yield return null;

            if (_currentCell == _endCell)
            {
                BuildPath(_startCell, _endCell, _cameFrom, _map, _pathTile);
                yield break;
            }

            if (_currentCell.IndexX > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX - 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexX < _cells.GetLength(0) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX + 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY - 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY < _cells.GetLength(1) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY + 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }
            _currentStep++;
            _frontier.RemoveAt(_bestIndex);
        }
        BuildPath(_startCell, _currentCell, _cameFrom, _map, _pathTile);
        yield return null;
    }
    #endregion

    #region AStar
    public static Cell[] FindAStarPath(Cell _startCell, Cell _endCell, Cell[,] cells)
    {
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();
        Cell[,] tempGridData = GridData.Instance.GetGridCopy();
        _startCell = tempGridData[_startCell.IndexX, _startCell.IndexY];
        _endCell = tempGridData[_endCell.IndexX, _endCell.IndexY];
        
        List<Cell> _frontier = new List<Cell> { _startCell };
        Cell _currentCell = null;
        Cell _neighbourCell = null;
        int _currentStep = 0, _bestIndex = 0;
        while (_frontier.Count > 0)
        {
            _bestIndex = GetBestCellIndex(_frontier.ToArray(), _endCell);
            _currentCell = _frontier[_bestIndex];

            if (_currentCell == _endCell)
            {
                return BuildPath(_startCell, _endCell, _cameFrom);
            }

            if (_currentCell.IndexX > 0 &&
                (_neighbourCell = tempGridData[_currentCell.IndexX - 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexX < tempGridData.GetLength(0) - 1 &&
                (_neighbourCell = tempGridData[_currentCell.IndexX + 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY > 0 &&
                (_neighbourCell = tempGridData[_currentCell.IndexX, _currentCell.IndexY - 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY < tempGridData.GetLength(1) - 1 &&
                (_neighbourCell = tempGridData[_currentCell.IndexX, _currentCell.IndexY + 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }
            _currentStep++;
            _frontier.RemoveAt(_bestIndex);
        }

        return new Cell[0];
    }

    public static IEnumerator DebugAstar(Cell _startCell, Cell _endCell, Cell[,] _cells, Tilemap _map, Tile _frontierTile, Tile _cameFromTile, Tile _pathTile, int _step = -1)
    {
        Dictionary<Cell, Cell> _cameFrom = new Dictionary<Cell, Cell>();
        List<Cell> _frontier = new List<Cell> { _startCell };
        Cell _currentCell = null;
        Cell _neighbourCell = null;
        int _currentStep = 0, _bestIndex = 0;
        while (_frontier.Count > 0 && _currentStep != _step)
        {
            _bestIndex = GetBestCellIndex(_frontier.ToArray(), _endCell);
            _currentCell = _frontier[_bestIndex];
            _map.SetTile(_currentCell.Position, _frontierTile);
            yield return null;

            if (_currentCell == _endCell)
            {
                BuildPath(_startCell, _endCell, _cameFrom, _map, _pathTile);
                yield break;
            }

            if (_currentCell.IndexX > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX - 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _neighbourCell.HeuristicCostFromStart = _currentCell.HeuristicCostFromStart + 1; 
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexX < _cells.GetLength(0) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX + 1, _currentCell.IndexY]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _neighbourCell.HeuristicCostFromStart = _currentCell.HeuristicCostFromStart + 1;
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY > 0 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY - 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _neighbourCell.HeuristicCostFromStart = _currentCell.HeuristicCostFromStart + 1;
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }

            if (_currentCell.IndexY < _cells.GetLength(1) - 1 &&
                (_neighbourCell = _cells[_currentCell.IndexX, _currentCell.IndexY + 1]).Cost < int.MaxValue &&
                !_cameFrom.ContainsKey(_neighbourCell))
            {
                _neighbourCell.HeuristicCostFromStart = _currentCell.HeuristicCostFromStart + 1;
                _frontier.Add(_neighbourCell);
                _cameFrom.Add(_neighbourCell, _currentCell);
            }
            _currentStep++;
            _frontier.RemoveAt(_bestIndex);
        }
        BuildPath(_startCell, _currentCell, _cameFrom, _map, _pathTile);
        yield return null;
    }
    #endregion

}
