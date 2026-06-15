#include<bits/stdc++.h>
using namespace std;
bool checkSort(int a[], int i, int n)
{
    if(i == n) return true;
    bool baki = checkSort(a, i+1, n);
    bool amar;
    if(a[i+1] >= a[i]) amar = true;
    else amar = false;

    return baki && amar;
}
int main()
{
    int n;
    cin >> n;
    int a[n];
    for(int i = 0; i < n; i++) {
        cin >> a[i];
    }
    bool ans = checkSort(a, 0, n-1);
    if(ans == true) cout << "True\n";
    else cout << "False\n";
    return 0;
}

