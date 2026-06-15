#include<bits/stdc++.h>
using namespace std;
int checkMax(int a[], int i, int n)
{
    if(i == n) return a[i];
    int maxx = checkMax(a, i+1, n);
    return max(a[i], maxx);
}
int main()
{
    int n;
    cin >> n;
    int a[n];
    for(int i = 0; i < n; i++) {
        cin >> a[i];
    }
    int ans = checkMax(a, 0, n-1);
    cout << ans << "\n";
    return 0;
}

