#pragma once
#include <vector>
#include "MyDouble.h"
#include "Matrix.h"

using namespace std;

class Layer {

private:
	int size, next_size;
	// Bias represent values of bias that go into next layer
	Matrix<MyDouble> values, weights, bias;

public:
	Layer();
	Layer(int size, int next_size = 0);

	void Resize(int size, int next_size);
	int Size() const;
	void SetValues(const Matrix<MyDouble>& values);
	Matrix<MyDouble>& GetValues() const;
	void SetWeights(const Matrix<MyDouble>& weights);
	Matrix<MyDouble>& GetWeights() const;
	void SetBias(const Matrix<MyDouble>& bias);
	Matrix<MyDouble>& GetBias() const;

	Matrix<MyDouble>& Propagate() const;
	void Reset();
};
