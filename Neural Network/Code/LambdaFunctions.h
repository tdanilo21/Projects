#pragma once
#include "MyDouble.h"

// Activation function is currently sigmoid.
MyDouble ActivationFunction(const MyDouble& x);
MyDouble ActivationFunctionDerivative(const MyDouble& x);

MyDouble Multiply(const MyDouble& a, const MyDouble& b);
MyDouble Add(const MyDouble& a, const MyDouble& b);
MyDouble Subtract(const MyDouble& a, const MyDouble& b);
