using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ConsoleApp1
{
    abstract class AbstractElement<T> where T : AbstractElement<T>
    {
        #region Abstract
        public abstract void Random();
        protected abstract T OperatorMultiply(T a);
        protected abstract T OperatorDevide(T a);
        protected abstract T OperatorAdd(T a);
        protected abstract T OperatorSubtract(T a);
        #endregion
        #region Operators
        public static T operator *(T a, AbstractElement<T> b)
        {
            return a.OperatorMultiply((T)b);
        }
        public static T operator /(T a, AbstractElement<T> b)
        {
            return a.OperatorDevide((T)b);
        }
        public static T operator +(T a, AbstractElement<T> b)
        {
            return a.OperatorAdd((T)b);
        }
        public static T operator -(T a, AbstractElement<T> b)
        {
            return a.OperatorSubtract((T)b);
        }
        #endregion
    }
    class MyDouble : AbstractElement<MyDouble>
    {
        private static Random rand = new Random();
        public double val;

        #region Constructors
        public MyDouble()
        {
            this.val = 0;
        }
        public MyDouble(double new_val)
        {
            this.val = new_val;
        }
        public MyDouble(MyDouble a)
        {
            this.val = a.val;
        }
        #endregion
        #region Funcitons
        public override void Random()
        {
            this.val = MyDouble.rand.Next();
        }
        protected override MyDouble OperatorMultiply(MyDouble a)
        {
            return new MyDouble(this.val * a.val);
        }
        protected override MyDouble OperatorDevide(MyDouble a)
        {
            return new MyDouble(this.val / a.val);
        }
        protected override MyDouble OperatorAdd(MyDouble a)
        {
            return new MyDouble(this.val + a.val);
        }
        protected override MyDouble OperatorSubtract(MyDouble a)
        {
            return new MyDouble(this.val - a.val);
        }
        #endregion
    }
    class Matrix<T> where T : AbstractElement<T>, new()
    {
        public T[,] data;
        private int rows, cols;

        #region Constructors
        public Matrix()
        {
            Construct(1, 1, new T());
        }
        public Matrix(int rows, int cols)
        {
            Construct(rows, cols, new T());
        }
        public Matrix(int rows, int cols, T defaultData)
        {
            Construct(rows, cols, defaultData);
        }
        public Matrix(Matrix<T> other)
        {
            Resize(other.rows, other.cols);
            SetData(other.data);
        }
        private void Construct(int rows, int cols, T defaultData)
        {
            Resize(rows, cols);
            Fill(defaultData);
        }
        #endregion
        #region Getters and setters
        public void Resize(int rows = 1, int cols = 1)
        {
            this.rows = rows;
            this.cols = cols;
            this.data = new T[rows, cols];
        }
        public Dims Dimensions()
        {
            return new Dims(this.rows, this.cols);
        }
        public void Fill(T data)
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] = data;
        }
        public void SetData(T[,] data)
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] = data[i, j];
        }
        public void Randomize()
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j].Random();
        }
        public T[] GetRow(int index)
        {
            Debug.Assert(index < this.rows);
            T[] result = new T[this.cols];
            for (int i = 0; i < this.cols; i++)
                result[i] = this.data[index, i];
            return result;
        }
        public T[] GetCol(int index)
        {
            Debug.Assert(index < this.cols);
            T[] result = new T[this.rows];
            for (int i = 0; i < this.rows; i++)
                result[i] = this.data[i, index];
            return result;
        }
        #endregion
        #region Helper functions
        public static T[] ToArray(Matrix<T> m)
        {
            T[] result = new T[m.rows * m.cols];
            for (int i = 0; i < m.rows; i++)
                for (int j = 0; j < m.cols; j++)
                    result[i * m.cols + j] = m.data[i, j];
            return result;
        }
        public static Matrix<T> FromArray(T[] arr)
        {
            Matrix<T> result = new Matrix<T>(arr.Length, 1);
            for (int i = 0; i < arr.Length; i++)
                result.data[i, 0] = arr[i];
            return result;
        }
        #endregion
        #region Functionalities
        public static Matrix<T> Product(Matrix<T> a, Matrix<T> b)
        {
            if (a.cols != b.rows)
                throw new Exception("Number of columns of matrix a has to be same as number of rows of matrix b!");
            Matrix<T> result = new Matrix<T>(a.rows, b.cols);
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < b.cols; j++)
                {
                    T sum = new T();
                    T[] r = a.GetRow(i), c = b.GetCol(j);
                    for (int t = 0; t < a.cols; t++)
                        sum += r[t] * c[t];
                    result.data[i, j] = sum;
                }
            }
            return result;
        }
        public static Matrix<T> Transpose(Matrix<T> m)
        {
            Matrix<T> result = new Matrix<T>(m.cols, m.rows);
            for (int i = 0; i < m.rows; i++)
                for (int j = 0; j < m.cols; j++)
                    result.data[j, i] = m.data[i, j];
            return result;
        }
        public static Matrix<T> ElementWise(Matrix<T> a, Matrix<T> b, Func<T, T, T> func)
        {
            if (a.rows != b.rows || a.cols != b.cols)
                throw new Exception("Dimensions of two matrices have to be same!");
            Matrix<T> result = new Matrix<T>(a.rows, a.cols);
            for (int i = 0; i < a.rows; i++)
                for (int j = 0; j < a.cols; j++)
                    result.data[i, j] = func(a.data[i, j], b.data[i, j]);
            return result;
        }
        public void Scale(Func<T, T> func)
        {
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    this.data[i, j] = func(this.data[i, j]);
        }
        #endregion

        public struct Dims
        {
            public int rows, cols;
            public Dims(int rows = 1, int cols = 1) { this.rows = rows; this.cols = cols; }
        }
    }
    class LambdaFunctions
    {
        // Activation function is currently sigmoid
        public static Func<MyDouble, MyDouble> ActivationFunction = x => { return new MyDouble(Math.Tanh(x.val)); };
        public static Func<MyDouble, MyDouble> ActivationFunctionDerivative = x => { return new MyDouble(1 - x.val * x.val); };

        public static Func<MyDouble, MyDouble, MyDouble> Multiply = (x, y) => { return new MyDouble(x * y); };
        public static Func<MyDouble, MyDouble, MyDouble> Devide = (x, y) => { return new MyDouble(x / y); };
        public static Func<MyDouble, MyDouble, MyDouble> Add = (x, y) => { return new MyDouble(x + y); };
        public static Func<MyDouble, MyDouble, MyDouble> Subtract = (x, y) => { return new MyDouble(x - y); };
    }
    class Layer
    {
        private int size, next_size;
        private Matrix<MyDouble> values, weights, bias;

        #region Constructors
        public Layer()
        {
            Resize(0, 0);
        }
        public Layer(int size, int next_size)
        {
            Resize(size, next_size);
        }
        #endregion
        #region Getters and setters
        public void Resize(int size, int next_size)
        {
            this.size = size;
            this.next_size = next_size;
            this.values = new Matrix<MyDouble>(size, 1);
            this.weights = new Matrix<MyDouble>(size, next_size);
            this.weights.Randomize();
            this.bias = new Matrix<MyDouble>(next_size, 1);
            this.bias.Randomize();
        }
        public int Size()
        {
            return this.size;
        }
        public void SetValues(Matrix<MyDouble> new_values)
        {
            if (new_values.Dimensions().rows != this.size || new_values.Dimensions().cols != 1)
                throw new Exception("New values must have same dimensions as the last one!");
            this.values = new Matrix<MyDouble>(new_values);
        }
        public Matrix<MyDouble> GetValues()
        {
            return new Matrix<MyDouble>(this.values);
        }
        public void SetWeights(Matrix<MyDouble> new_weights)
        {
            if (new_weights.Dimensions().rows != this.size || new_weights.Dimensions().cols != this.next_size)
                throw new Exception("New weights must have same dimensions as the last one!");
            this.weights = new Matrix<MyDouble>(new_weights);
        }
        public Matrix<MyDouble> GetWeights()
        {
            return new Matrix<MyDouble>(this.weights);
        }
        public void SetBias(Matrix<MyDouble> new_bias)
        {
            if (new_bias.Dimensions().rows != this.next_size || new_bias.Dimensions().cols != 1)
                throw new Exception("New bias must have same dimensions as the last one!");
            this.bias = new Matrix<MyDouble>(new_bias);
        }
        public Matrix<MyDouble> GetBias()
        {
            return new Matrix<MyDouble>(this.bias);
        }
        #endregion
        #region Funcitons
        public Matrix<MyDouble> Propagate()
        {
            Matrix<MyDouble> weights_t = Matrix<MyDouble>.Transpose(this.weights);
            Matrix<MyDouble> next_values = Matrix<MyDouble>.Product(weights_t, this.values);
            next_values = Matrix<MyDouble>.ElementWise(next_values, this.bias, LambdaFunctions.Add);
            next_values.Scale(LambdaFunctions.ActivationFunction);
            return next_values;
        }
        public void Reset()
        {
            this.values.Fill(new MyDouble());
        }
        #endregion
    }
    class NeuralNetwork
    {
        private int size;
        private Layer[] layers;
        private Matrix<MyDouble> maxInput, maxOutput;
        private static MyDouble learningRate = new MyDouble(0.15);

        #region Constructors
        public NeuralNetwork()
        {
            Resize(0);
        }
        public NeuralNetwork(int size, int[] layout, double[] maxInputData, double[] maxOutputData)
        {
            Resize(size);
            Setlayout(layout);
            SetMaxData(maxInputData, maxOutputData);
        }
        #endregion
        #region Helper functions
        private static MyDouble[] FromDouble(double[] arr)
        {
            MyDouble[] result = new MyDouble[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                result[i] = new MyDouble(arr[i]);
            return result;
        }
        private static double[] ToDouble(MyDouble[] arr)
        {
            double[] result = new double[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                result[i] = arr[i].val;
            return result;
        }
        #endregion
        #region Setters
        private void Resize(int size)
        {
            this.size = size;
            this.layers = new Layer[size];
        }
        private void Setlayout(int[] layout)
        {
            for (int i = 0; i < layout.Length; i++)
            {
                int next_size = (i == layout.Length - 1 ? 0 : layout[i + 1]);
                this.layers[i] = new Layer(layout[i], next_size);
            }
        }
        private void SetMaxData(double[] maxInputData, double[] maxOutputData)
        {
            Debug.Assert(this.layers[0].Size() == maxInputData.Length);
            Debug.Assert(this.layers[this.size - 1].Size() == maxOutputData.Length);
            this.maxInput = Matrix<MyDouble>.FromArray(NeuralNetwork.FromDouble(maxInputData));
            this.maxOutput = Matrix<MyDouble>.FromArray(NeuralNetwork.FromDouble(maxOutputData));
        }
        #endregion
        #region Functionalities
        private void Reset()
        {
            for (int i = 0; i < this.size; i++)
                this.layers[i].Reset();
        }
        private void FeedForward(Matrix<MyDouble> input)
        {
            this.layers[0].SetValues(input);
            for (int i = 0; i < this.size - 1; i++)
            {
                Matrix<MyDouble> next_values = this.layers[i].Propagate();
                this.layers[i + 1].SetValues(next_values);
            }
        }
        private Matrix<MyDouble> GetOutput()
        {
            return this.layers[this.size - 1].GetValues();
        }
        private void BackPropagate(Matrix<MyDouble> answer)
        {
            // TODO
            // Calculating errors for the last layer:
            Matrix<MyDouble>[] errors = new Matrix<MyDouble>[this.size];
            Matrix<MyDouble> last_values = this.layers[this.size - 1].GetValues();
            errors[this.size - 1] = Matrix<MyDouble>.ElementWise(answer, last_values, LambdaFunctions.Subtract);
            errors[this.size - 1].Scale(x => x * x);

            // Calculating derivatives for the last layer:
            Matrix<MyDouble>[] derivatives = new Matrix<MyDouble>[this.size];
            derivatives[this.size - 1] = Matrix<MyDouble>.ElementWise(answer, last_values, LambdaFunctions.Subtract);
            derivatives[this.size - 1].Scale(x => x * new MyDouble(2));

            // Back Propagating:
            for (int i = this.size - 2; i >= 0; i--)
            {
                Matrix<MyDouble> values_t = Matrix<MyDouble>.Transpose(this.layers[i].GetValues());
                Matrix<MyDouble> weights = this.layers[i].GetWeights();
                Matrix<MyDouble> bias = this.layers[i].GetBias();
                Matrix<MyDouble> next_values = this.layers[i + 1].GetValues();

                // Calculating gradient descent:
                Matrix<MyDouble> gradient = new Matrix<MyDouble>(next_values);
                gradient.Scale(LambdaFunctions.ActivationFunctionDerivative);

                // Calculating delta weights:
                Matrix<MyDouble> delta_weights = Matrix<MyDouble>.ElementWise(errors[i + 1], gradient, LambdaFunctions.Multiply);
                delta_weights = Matrix<MyDouble>.ElementWise(delta_weights, derivatives[i + 1], LambdaFunctions.Multiply);
                delta_weights = Matrix<MyDouble>.Product(delta_weights, values_t);
                delta_weights.Scale(x => x * NeuralNetwork.learningRate);
                delta_weights = Matrix<MyDouble>.Transpose(delta_weights);

                // Adjusting weights:
                Matrix<MyDouble> new_weights = Matrix<MyDouble>.ElementWise(weights, delta_weights, LambdaFunctions.Add);
                this.layers[i].SetWeights(new_weights);

                // Calculating delta biases:
                Matrix<MyDouble> delta_bias = Matrix<MyDouble>.ElementWise(errors[i + 1], gradient, LambdaFunctions.Multiply);
                delta_bias = Matrix<MyDouble>.ElementWise(delta_bias, derivatives[i + 1], LambdaFunctions.Multiply);
                delta_bias.Scale(x => x * NeuralNetwork.learningRate);

                // Adjusting biases:
                Matrix<MyDouble> new_bias = Matrix<MyDouble>.ElementWise(bias, delta_bias, LambdaFunctions.Add);
                this.layers[i].SetBias(new_bias);

                // Calculating derivatives:
                derivatives[i] = Matrix<MyDouble>.ElementWise(derivatives[i + 1], gradient, LambdaFunctions.Multiply);
                derivatives[i] = Matrix<MyDouble>.Product(weights, derivatives[i]);

                // Calculating errors:
                errors[i] = Matrix<MyDouble>.Product(weights, errors[i + 1]);
            }
        }
        public double[] Run(double[] input)
        {
            if (input.Length != this.layers[0].Size())
                throw new Exception("Input lenght must be same as number of neurons in the first layer!");
            for (int i = 0; i < input.Length; i++)
                if (input[i] > this.maxInput.data[i, 0].val)
                    throw new Exception("Value of i-th input must be less than or equal to the max value for that neuron in the first layer!");

            Matrix<MyDouble> input_mat = Matrix<MyDouble>.FromArray(NeuralNetwork.FromDouble(input));
            input_mat = Matrix<MyDouble>.ElementWise(input_mat, this.maxInput, LambdaFunctions.Devide);
            Reset();
            FeedForward(input_mat);
            Matrix<MyDouble> output_mat = GetOutput();
            output_mat = Matrix<MyDouble>.ElementWise(output_mat, this.maxOutput, LambdaFunctions.Multiply);
            double[] output = NeuralNetwork.ToDouble(Matrix<MyDouble>.ToArray(output_mat));
            return output;
        }
        public double[] Train(double[] input, double[] answer)
        {
            if (answer.Length != this.layers[this.size - 1].Size())
                throw new Exception("Answer lenght must be same as number of neurons in the last layer!");
            for (int i = 0; i < answer.Length; i++)
                if (answer[i] > this.maxOutput.data[i, 0].val)
                    throw new Exception("Value of i-th answer must be less than or equal to the max value for that neuron in the last layer!");

            double[] output = Run(input);
            Matrix<MyDouble> answer_mat = Matrix<MyDouble>.FromArray(NeuralNetwork.FromDouble(answer));
            answer_mat = Matrix<MyDouble>.ElementWise(answer_mat, this.maxOutput, LambdaFunctions.Devide);
            BackPropagate(answer_mat);
            return output;
        }
        #endregion
    }
    class Program
    {
        // TODO:
        static void Print(Matrix<MyDouble> m)
        {
            for (int i = 0; i < m.Dimensions().rows; i++)
            {
                MyDouble[] row = m.GetRow(i);
                for (int j = 0; j < m.Dimensions().cols; j++)
                    Console.Write($"{row[j].val} ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        struct Data
        {
            public double[] input, answer;
            public Data(int n, int m)
            {
                input = new double[n];
                answer = new double[m];
            }
        }
        static void Print(double[] input, double[] output, double[] answer, int tc)
        {
            Console.Write($"Count: {tc}; Input: [");
            for (int i = 0; i < input.Length; i++)
                Console.Write($"{input[i]}" + (i < input.Length - 1 ? ", " : "]; "));
            Console.Write("Output: [");
            for (int i = 0; i < output.Length; i++)
                Console.Write($"{output[i]}" + (i < output.Length - 1 ? ", " : "]; "));
            Console.Write("Answer: [");
            for (int i = 0; i < answer.Length; i++)
                Console.Write($"{answer[i]}" + (i < answer.Length - 1 ? ", " : "]; "));
            if (tc % 2 == 0) Console.WriteLine();

        }
        static void Learn(NeuralNetwork Bot, Data[] trening, int tc)
        {
            int ind = new Random().Next(4);
            if (ind == 4)
                throw new Exception("Index was outside of bounds!");
            double[] output = Bot.Train(trening[ind].input, trening[ind].answer);
            Print(trening[ind].input, output, trening[ind].answer, tc + 1);
        }
        static void Main(string[] args)
        {
            #region Prep work
            int[] layout = { 2, 4, 1 };
            Data[] trening = new Data[4];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Data d = new Data(layout[0], layout[layout.Length - 1]);
                    d.input[0] = i;
                    d.input[1] = j;
                    d.answer[0] = i ^ j;
                    trening[2 * i + j] = d;
                }
            }
            double[] maxInput = { 1, 1 }, maxOutput = { 1 };
            NeuralNetwork Bot = new NeuralNetwork(layout.Length, layout, maxInput, maxOutput);
            #endregion
            int t = int.Parse(Console.ReadLine());
            for (int i = 0; i < t; i++)
                Learn(Bot, trening, i);
        }
    }
}
