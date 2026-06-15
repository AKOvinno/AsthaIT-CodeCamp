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
class LinkedList
{
public:
    Node *head = NULL;
    Node *tail = NULL;
    void insertAtTail(int val)
    {
        Node *newNode = new Node(val);
        if (this->head == NULL)
        {
            this->head = newNode;
            this->tail = newNode;
        }
        else
        {
            this->tail->next = newNode;
            this->tail = this->tail->next;
        }
    }
    void deleteAtHead()
    {
        if (head == NULL)
            return;
        if (head->next == NULL)
        {
            tail = NULL;
        }
        Node *deleteNode = head;
        head = head->next;
        delete deleteNode;
    }
};
class MyQueue
{
private:
    LinkedList list;
    int sz = 0;

public:
    void push(int val)
    {
        list.insertAtTail(val);
        sz++;
    }
    void pop()
    {
        list.deleteAtHead();
        if (sz)
            sz--;
    }
    int front()
    {
        return list.head->val;
    }
    int size()
    {
        return sz;
    }
    bool empty()
    {
        return list.head == NULL;
    }
};
int main()
{
    MyQueue q;
    q.push(10);
    q.push(20);
    q.push(30);
    if (!q.empty())
    {
        cout<<q.front()<<endl;
    }
    q.pop();
    if (!q.empty())
    {
        cout<<q.front()<<endl;
    }
    q.pop();
    if (!q.empty())
    {
        cout<<q.front()<<endl;
    }
    q.pop();
    q.pop();
    q.pop();
    if (!q.empty())
    {
        cout<<q.front()<<endl;
    }
    return 0;
}
