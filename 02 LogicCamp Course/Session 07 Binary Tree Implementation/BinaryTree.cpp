#include<bits/stdc++.h>
using namespace std;
class Node
{
    public:
        int val;
        Node* left;
        Node* right;
    Node(int val)
    {
        this->val = val;
        this->left = NULL;
        this->right = NULL;
    }
};
void preorder(Node* root)
{
    if (root == NULL) return;
    cout<<root->val<<" ";
    preorder(root->left);
    preorder(root->right);
}
void postorder(Node* root)
{
    if (root == NULL) return;
    postorder(root->left);
    postorder(root->right);
    cout<<root->val<<" ";
}
void inorder(Node* root)
{
    if (root == NULL) return;
    inorder(root->left);
    cout<<root->val<<" ";
    inorder(root->right);
}
void bfs(Node* root)
{
    if (root == NULL) return;
    queue<pair<Node*,int>> q;
    q.push({root, 0});
    while(!q.empty())
    {
        pair<Node*, int> p = q.front();
        Node* cur = p.first;
        int level = p.second;
        q.pop();
        cout<<cur->val<<" - "<<level<<endl;
        if(cur->left)
        {
            q.push({cur->left, level+1});
        }
        if(cur->right)
        {
            q.push({cur->right, level+1});
        }
    }
}
void input(Node* &root)
{
    int val; cin>>val;
    if (val == -1) return;
    root = new Node(val);
    queue<Node*> q;
    q.push(root);
    while(!q.empty())
    {
        Node* cur = q.front();
        q.pop();
        int left, right;
        cin>>left>>right;
        Node* leftNode = NULL;
        Node* rightNode = NULL;
        if (left != -1) leftNode = new Node(left);
        if (right != -1) rightNode = new Node(right);
        cur->left = leftNode;
        cur->right = rightNode;
        if (leftNode) q.push(leftNode);
        if (rightNode) q.push(rightNode);
    }
}
int main()
{
    Node* root = NULL;
    input(root);
    bfs(root);
    return 0;
}
