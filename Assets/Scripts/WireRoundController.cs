using UnityEngine;
using UnityEngine.UI;
using System;

public class WireRoundController : MonoBehaviour
{
    public Image[] insulationSlots;
    public WireSocket[] leftSockets;
    public WireSocket[] rightSockets;

    public Action OnRoundSolved;

    private WireSocket selectedSocket;
    private int connectedCount = 0;

    public void ResetRound()
    {
        connectedCount = 0;
        selectedSocket = null;

        if (insulationSlots != null)
        {
            foreach (var slot in insulationSlots)
            {
                if (slot != null)
                {
                    Color c = slot.color;
                    c.a = 0f;
                    slot.color = c;
                }
            }
        }

        foreach (var s in leftSockets)
        {
            s.isConnected = false;
            s.SetHighlight(false);
        }
        foreach (var s in rightSockets)
        {
            s.isConnected = false;
            s.SetHighlight(false);
        }
    }

    public void OnSocketClicked(WireSocket socket)
    {
        if (socket.isConnected) return;

        if (selectedSocket == null)
        {
            selectedSocket = socket;
            socket.SetHighlight(true);
            return;
        }

        if (selectedSocket == socket)
        {
            socket.SetHighlight(false);
            selectedSocket = null;
            return;
        }

        if (selectedSocket.isLeftSide == socket.isLeftSide)
        {
            selectedSocket.SetHighlight(false);
            selectedSocket = socket;
            socket.SetHighlight(true);
            return;
        }

        TryConnect(selectedSocket, socket);
        selectedSocket.SetHighlight(false);
        selectedSocket = null;
    }

    void TryConnect(WireSocket a, WireSocket b)
    {
        WireSocket left = a.isLeftSide ? a : b;
        WireSocket right = a.isLeftSide ? b : a;

        if (left.colorId == right.colorId)
        {
            ShowInsulation(left.colorId);

            left.isConnected = true;
            right.isConnected = true;
            connectedCount++;

            if (connectedCount >= leftSockets.Length)
                OnRoundSolved?.Invoke();
        }
    }

    void ShowInsulation(int colorId)
    {
        if (insulationSlots == null || colorId < 0 || colorId >= insulationSlots.Length) return;
        if (insulationSlots[colorId] == null) return;

        Color c = insulationSlots[colorId].color;
        c.a = 1f;
        insulationSlots[colorId].color = c;
    }
}