using UnityEngine;

public class Matrix
{

  public int row, col;
  readonly float[][] data;

  public float[][] Data
  {
    get
    {
      return data;
    }
  }

  public Matrix(int row, int col, float num = 0)
  {
    this.row = row;
    this.col = col;

    data = new float[row][];
    for (int i = 0; i < row; i++)
    {
      data[i] = new float[col];
      for (int j = 0; j < col; j++)
      {
        data[i][j] = num;
      }
    }
  }

  public Matrix(float[][] data)
  {
    row = data.Length;
    col = (data.Length == 0) ? 0 : data[0].Length;

    this.data = data;
  }
  public float this[int row, int col]
  {
    get { return data[row][col]; }
    set { data[row][col] = value; }
  }

  public static Matrix MultiplyMatrixtoConst(float value, Matrix matrix)
  {
    Matrix newMatrix = matrix;
    for (int row = 0; row < matrix.row; row++)
    {
      for (int col = 0; col < matrix.col; col++)
      {
        newMatrix[row, col] *= value;
      }
    }
    return newMatrix;
  }

  public static Matrix AddMatrixtoMatrix(Matrix a, Matrix b, bool positive)
  {
    Matrix newMatrix = a;
    if (a.col == b.col && a.row == b.row)
    {
      for (int i = 0; i < b.row; i++)
      {
        for (int j = 0; j < b.col; j++)
        {
          newMatrix[i, j] += (positive) ? b[i, j] : -b[i, j];
        }
      }
    }
    else
    {
      Debug.Log("Can't Add");
    }
    return newMatrix;
  }

  public Matrix CollapseCols()
  {
    Matrix newMatrix = new Matrix(row, 1);
    for (int i = 0; i < row; i++)
    {
      float sum = 0;
      foreach (float point in data[i])
      {
        sum += point;
      }
      newMatrix[i, 0] = sum;
    }
    return newMatrix;
  }
  public Matrix CollapseRows()
  {
    return T().CollapseCols().T();
  }
  public Matrix Randomise()
  {
    Matrix newMatrix = this;
    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        newMatrix[i, j] = Random.value * 2 - 1;
      }
    }
    return newMatrix;
  }

  public Matrix T()
  {
    Matrix newMatrix = new Matrix(col, row);

    for (int i = 0; i < row; i++)
    {
      for (int j = 0; j < col; j++)
      {
        newMatrix[j, i] = this[i, j];
      }
    }
    return newMatrix;
  }

  public static Matrix operator +(Matrix a, Matrix b)
  {
    return AddMatrixtoMatrix(a, b, true);
  }
  public static Matrix operator -(Matrix a, Matrix b)
  {
    return AddMatrixtoMatrix(a, b, false);
  }

  public static Matrix Sigmoid(Matrix a)
  {
    Matrix newMatrix = new Matrix(a.row, a.col);
    for (int i = 0; i < a.row; i++)
    {
      for (int j = 0; j < a.col; j++)
      {
        newMatrix[i, j] = 1 / (1 + Mathf.Exp(-newMatrix[i, j]));
      }
    }
    return newMatrix;
  }
  public static Matrix Mutate(Matrix a, float mutationRate)
  {
    Matrix newMatrix = new Matrix(a.row, a.col);
    for (int i = 0; i < newMatrix.row; i++)
    {
      for (int j = 0; j < newMatrix.col; j++)
      {
        newMatrix[i, j] = (Random.value < mutationRate) ? a[i, j] + (Random.value * 0.2f - 0.1f) : a[i, j];
      }
    }
    return newMatrix;
  }
  public static Matrix[] MutateArray(Matrix[] a, float mutationRate)
  {
    for (int i = 0; i < a.Length; i++)
    {
      a[i] = Mutate(a[i], mutationRate);
    }
    return a;
  }

  public static Matrix PickFromTwo(Matrix a, Matrix b)
  {
    Matrix newMatrix = new Matrix(a.row, a.col);
    if (a.row != b.row || a.col != b.col) { return null; }
    for (int i = 0; i < newMatrix.row; i++)
    {
      for (int j = 0; j < newMatrix.col; j++)
      {
        newMatrix[i, j] = (Random.value > 0.5f) ? a[i, j] : b[i, j];
      }
    }
    return newMatrix;
  }
  public static Matrix[] PickFromTwoMatrix(Matrix[] a, Matrix[] b)
  {
    Matrix[] newMatrixArray = new Matrix[a.Length];
    if (a.Length != b.Length) { Debug.Log("Something's Wrong"); return null; }
    for (int i = 0; i < newMatrixArray.Length; i++)
    {
      newMatrixArray[i] = PickFromTwo(a[i], b[i]);
    }
    return newMatrixArray;
  }

  public static Matrix operator *(float a, Matrix b)
  {
    return MultiplyMatrixtoConst(a, b);
  }
  public static Matrix operator *(Matrix b, float a)
  {
    return MultiplyMatrixtoConst(a, b);
  }

  public static Matrix operator *(Matrix a, Matrix b)
  {
    Matrix newMatrix = new Matrix(a.row, b.col);

    for (int i = 0; i < newMatrix.row; i++)
    {
      for (int j = 0; j < newMatrix.col; j++)
      {
        float sum = 0;
        for (int k = 0; k < a.col; k++)
        {
          sum += a[i, k] * b[k, j];
        }
        newMatrix[i, j] = sum;
      }
    }
    return newMatrix;
  }

  public Matrix Copy()
  {
    float[][] newData = new float[row][];

    for (int rowIndex = 0; rowIndex < row; rowIndex++)
    {
      newData[rowIndex] = new float[col];
      for (int colIndex = 0; colIndex < col; colIndex++)
      {
        newData[rowIndex][colIndex] = data[rowIndex][colIndex];
      }
    }

    return new Matrix(newData);
  }

  // Copies an array of matrices
  public static Matrix[] Copy(Matrix[] matrixSet)
  {
    Matrix[] newMatrixSet = new Matrix[matrixSet.Length];

    for (int matrixIndex = 0; matrixIndex < matrixSet.Length; matrixIndex++)
    {
      newMatrixSet[matrixIndex] = matrixSet[matrixIndex].Copy();
    }

    return newMatrixSet;
  }
}