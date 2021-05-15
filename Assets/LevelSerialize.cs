using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//(1|1, 1|2, ...)(...)

[CreateAssetMenu(fileName ="level", menuName ="Level")]
public class LevelSerialize: ScriptableObject{
	public int sizeX;
	public int sizeY;
	public int id;
	public int swapLinit;
	public int stepLimit;
	public string map;
}
