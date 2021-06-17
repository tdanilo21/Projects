#pragma once
#include <vector>
#include "Matrix.h"

using namespace std;

class MyDouble {
public:
	double val;

	MyDouble(double val = 0);
	MyDouble(const MyDouble& other);

	static MyDouble& Random();
	static MyDouble& DotProduct(const vector<MyDouble>& a, const vector<MyDouble>& b);

	MyDouble& operator=(const MyDouble& other);
	MyDouble& operator+=(const MyDouble& other);
	MyDouble& operator*(const MyDouble& other) const;
	MyDouble& operator+(const MyDouble& other) const;
};

class Layer {

private:
	int size, next_size;
	// Bias represent values of bias that go into next layer
	Matrix<MyDouble> values, weights, bias;

public:
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
