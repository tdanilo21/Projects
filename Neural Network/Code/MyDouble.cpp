#include <vector>
#include <assert.h>
#include "MyDouble.h"

using namespace std;

MyDouble::MyDouble(double val) { this->val = val; }
MyDouble::MyDouble(const MyDouble& other) { *this = other; }
MyDouble& MyDouble::Random() { return *new MyDouble(((double)rand() / (double)RAND_MAX) * 2 - 1); }
MyDouble& MyDouble::DotProduct(const vector<MyDouble>& a, const vector<MyDouble>& b) {
	assert(a.size() == b.size());
	MyDouble result = *new MyDouble();
	for (int i = 0; i < a.size(); i++)
		result += a[i] * b[i];
	return result;
}
MyDouble& MyDouble::operator=(const MyDouble& other) { this->val = other.val; return *this; }
MyDouble& MyDouble::operator+=(const MyDouble& other) { this->val += other.val; return *this; }
MyDouble& MyDouble::operator*(const MyDouble& other) const { return *new MyDouble(this->val * other.val); }
MyDouble& MyDouble::operator+(const MyDouble& other) const { return *new MyDouble(this->val + other.val); }
MyDouble& MyDouble::operator-(const MyDouble& other) const { return *new MyDouble(this->val - other.val); }
