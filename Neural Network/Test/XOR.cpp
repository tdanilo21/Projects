#include <iostream>
#include <vector>
#include <ctime>
#include <chrono>
#include <assert.h>
#include <algorithm>

#include "NeuralNetwork.h"

using namespace std;

struct Data {
	vector<double> input, answer;
};

Net* Bot;
vector<Data> trening;
int n, m;

void Print(const vector<double>& input, const vector<double>& output, const vector<double>& answer, int tc) {
	cout << "Count: " << tc << "; ";
	cout << "Input: [";
	for (int i = 0; i < input.size(); i++)
		cout << input[i] << (i < input.size() - 1 ? ", " : "]; ");
	cout << "Output: [";
	for (int i = 0; i < output.size(); i++)
		cout << output[i] << (i < output.size() - 1 ? ", " : "]; ");
	if (answer.size()) cout << "Answer: [";
	for (int i = 0; i < answer.size(); i++)
		cout << answer[i] << (i < answer.size() - 1 ? ", " : "]; ");
	if (tc % 2 == 0) cout << endl;
}

void Learn(int tc) {

	double r = ((double)rand() / (double)RAND_MAX) * 4.0f;
	int ind = min((int)r, 3);
	vector<double> output;
	Bot->Train(trening[ind].input, output, trening[ind].answer);
	Print(trening[ind].input, output, trening[ind].answer, tc + 1);
}

void Test(const vector<double>& input, int tc) {

	vector<double> output;
	Bot->Run(input, output);
	Print(input, output, {}, tc + 1);
}

int main()
{
	n = 2; m = 1;
	Bot = new Net({ 2, 4, 1 });
	for (int a = 0; a < 2; a++) {
		for (int b = 0; b < 2; b++) {
			Data d;
			d.input = { (double)a, (double)b };
			d.answer = { (double)(a ^ b) };
			trening.push_back(d);
		}
	}

	int t; cin >> t;

	auto start = chrono::steady_clock::now();
	for (int i = 0; i < t; i++) Learn(i);

	auto end = chrono::steady_clock::now();
	auto elapsed1 = chrono::duration_cast<chrono::seconds>(end - start);
	cout << elapsed1.count() << " seconds" << endl;

	cin >> t;
	vector<vector<double> > test(t, vector<double>(n));
	for (int i = 0; i < t; i++)
		for (int j = 0; j < n; j++)
			cin >> test[i][j];
	start = chrono::steady_clock::now();
	for (int i = 0; i < t; i++) Test(test[i], i);
	end = chrono::steady_clock::now();
	auto elapsed2 = chrono::duration_cast<chrono::milliseconds>(end - start);
	cout << elapsed2.count() << " milliseconds" << endl;

	return 0;
}
