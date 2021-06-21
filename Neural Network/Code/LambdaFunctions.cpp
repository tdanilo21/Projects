#include "LambdaFunctions.h"

using namespace std;

// Activation function is currently sigmoid.
MyDouble ActivationFunction(const MyDouble& x) { return MyDouble(tanh(x.val)); }
MyDouble ActivationFunctionDerivative(const MyDouble& x) { return MyDouble(1.0f - x.val * x.val); }

MyDouble Multiply(const MyDouble& a, const MyDouble& b) { return a * b; }
MyDouble Add(const MyDouble& a, const MyDouble& b) { return a + b; }
MyDouble Subtract(const MyDouble& a, const MyDouble& b) { return a - b; }
