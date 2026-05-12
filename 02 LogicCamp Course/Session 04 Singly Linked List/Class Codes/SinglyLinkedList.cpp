#include <bits/stdc++.h>
using namespace std;
class Node
{
public:
    int val;
    Node *next;
    Node(int val)
    {
        this->val = val;
        this->next = NULL;
    }
};
void insertAtTail(Node *&head, Node *&tail, int val)
{
    Node *newNode = new Node(val);
    if (head == NULL)
    {
        head = newNode;
        tail = newNode;
    }
    else
    {
        tail->next = newNode;
        tail = tail->next;
    }
}
void print(Node *&head)
{
    Node *tmp = head;
    while (tmp != NULL)
    {
        cout << tmp->val << " ";
        tmp = tmp->next;
    }
    cout << endl;
}
void deleteIndex(Node *&head, int idx)
{
    if (idx == 0)
    {
        Node *deleteNode = head;
        head = head->next;
        delete deleteNode;
        return;
    }
    Node *tmp = head;
    for (int i = 0; i < idx - 1; i++)
    {
        tmp = tmp->next;
    }
    Node *deleteNode = tmp->next;
    tmp->next = deleteNode->next;
    delete deleteNode;
}
Node* reverse(Node *head)
{
    if (head == NULL || head->next == NULL)
    {
        return head;
    }
    Node* tmp = head->next;
    Node* newHead = reverse(head->next);
    tmp->next = head;
    head->next = NULL;
    return newHead;
}
int main()
{
    int n;
    Node *head = NULL;
    Node *tail = NULL;
    while (cin >> n)
    {
        insertAtTail(head, tail, n);
    }
    head = reverse(head);
    print(head);
    return 0;
}
