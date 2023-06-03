using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEngine.Mathf;


namespace Utilities {
	public static class VectorUtil
	{
		/// <summary>
		/// 두 벡터 지점의 중점을 반환하는 메소드
		/// </summary>
		/// <param name="aPos">지점 a</param>
		/// <param name="bPos">지점 b</param>
		/// <param name="zPos">반환된 중점의 z좌표(기본값 0.1f)</param>
		/// <returns>지점 a와 b 사이의 중점을 저장한 Vector3 구조체</returns>
		public static Vector3 GetMidPoint2D(Vector3 aPos, Vector3 bPos, float zPos = 0.1f)
		{
			var midPointX = (aPos.x + bPos.x) / 2;
			var midPointY = (aPos.y + bPos.y) / 2;

			return new Vector3(midPointX, midPointY, zPos);
		}

		/// <summary>
		/// 시작 지점에서 끝 지점까지 선분을 그렸을 때,
		/// 선분의 각을 Vector3 구조체의 z축 값에 저장하여 반환하는 메소드
		/// * 해당 메소드는 Unity 2D 플랫폼에서 사용함이 전제된다.
		/// </summary>
		/// <param name="fromPos">시작 지점</param>
		/// <param name="endPos">끝 지점</param>
		/// <returns>Z축 회전 값을 저장한 Vector3 구조체</returns>
		public static Vector3 GetZRotation2D(Vector3 fromPos, Vector3 endPos)
		{
			var deltaX = endPos.x - fromPos.x;
			var deltaY = endPos.y - fromPos.y;
			var gradient = deltaY / deltaX;

			var zRotation = Rad2Deg * Atan(gradient);

			switch(deltaX) {
				case > 0 when deltaY >= 0: //제1사분면, 0~90
					break;   //does nothing
				case < 0 when deltaY >= 0: //제2사분면, 90~180
				case < 0 when deltaY < 0:  //제3사분면, 180~270
					zRotation += 180;
					break;
				case > 0 when deltaY < 0:   //제4사분면, 270~360(0)
					zRotation += 360;
					break;
			}

			//Debug.Log($"{deltaX}, {deltaY}, {gradient}, {zRotation}");

			return new Vector3(0, 0, zRotation);
		}

		/// <summary>
		/// 오일러 각도와 길이 값을 매개변수로 받아 해당
		/// 선분(->벡터로 취급)의 x성분과 y성분 값을 저장한 Vector3 구조체를 반환하는 메소드
		/// </summary>
		/// <param name="eulerAngleZ">각도값(Z축, Euler Degree)</param>
		/// <param name="length">길이</param>
		/// <returns>x성분과 y성분 값을 저장한 Vector3 구조체</returns>
		public static Vector3 GetVectorComponents2D(float eulerAngleZ, float length)
		{
			var xComponent = Cos(eulerAngleZ * Deg2Rad) * length;
			var yComponent = Sin(eulerAngleZ * Deg2Rad) * length;

			return new Vector3(xComponent, yComponent);
		}

		/// <summary>
		/// (x 혹은 y)성분의 크기(길이)와 각도를 매개변수로 받아
		/// 해당 선분/벡터의 길이/크기를 구하는 메소드
		/// </summary>
		/// <param name="componentLength">(x 혹은 y)성분의 크기(길이)</param>
		/// <param name="angle">각도(Degree)</param>
		/// <param name="componentAxis">x성분이면 "x", y성분이면 "y"</param>
		/// <exception cref="DivideByZeroException">각도 값에 따라 Sin 혹은 Cos값이 0이 되었는데 나누기를 시도한 경우</exception>
		/// <returns>성분들을 합성한 벡터의 크기(길이)</returns>
		public static float GetVectorLength(float componentLength, float angle, string componentAxis)
		{
			var length = componentAxis switch {
				"x" => componentLength / Abs(Cos(angle * Deg2Rad)),
				"y" => componentLength / Abs(Sin(angle * Deg2Rad)),
				_   => throw new ArgumentOutOfRangeException("")
			};
			
			return length;
		}

		/// <summary>
		/// GetVectorLength 메소드에서 각도 값에 따라
		/// 0으로 나누게 되는 상황이 발생하지 않도록 보정/보완한 메소드
		/// </summary>
		/// <param name="xComponentLength">x성분</param>
		/// <param name="yComponentLength">y성분</param>
		/// <param name="angle">각도 (360도가 넘는 각도값은 해당 메소드 내에서 360미만으로 만든다)</param>
		/// <returns></returns>
		public static float GetVectorLength2(float xComponentLength, float yComponentLength, float angle)
		{
			while(angle < 0)
				angle += 360;
			
			while(angle > 360)
				angle -= 360;

			// bug angle이 135 ~ 315일 경우 값이 반대 방향으로 나옴 => 해당 범위의 경우를 따로 나누어 전체 성분값 - xComponentLength 등을 인자로 넘기기
			var length = angle switch {
				(>= 45  and < 135) or (>= 225 and < 315)                    => GetVectorLength(yComponentLength, angle, "y"),
				(>= 135 and < 225) or (>= 315 and < 360) or (>= 0 and < 45) => GetVectorLength(xComponentLength, angle, "x"),
				_                                                           => throw new ArgumentOutOfRangeException($"angle({angle}) is out of range")
			};

			return length;
		}

		/// <summary>
		/// GetVectorLength 메소드에서 각도 값에 따라
		/// 0으로 나누게 되는 상황이 발생하지 않도록 보정/보완하고
		/// GetVectorLength 메소드에서 각도 값이 135~315인 경우
		/// 결과값이 뒤집혀 반환되는 문제를 개선한 메소드
		/// 
		/// 추가로 오브젝트의 너비, 높이를 필요로 한다.
		/// </summary>
		/// <param name="xComponentLength">x성분</param>
		/// <param name="yComponentLength">y성분</param>
		/// <param name="angle">각도 (0~360의 범위를 벗어난 경우 해당 메소드 내에서 값을 조정한다)</param>
		/// <param name="objSize"></param>
		/// <param name="objHeight"></param>
		/// <returns></returns>
		public static float GetVectorLengthInObject(float xComponentLength, float yComponentLength, Vector3 objSize, float angle)
		{
			angle = Util.CorrectEulerAngleValue(angle);
			
			var length = angle switch {
				(>= 45  and < 135)                    => GetVectorLength(yComponentLength, angle, "y"),
				(>= 135 and < 225)                    => GetVectorLength(objSize.x - xComponentLength, angle, "x"),
				(>= 225 and < 315)                    => GetVectorLength(objSize.y - yComponentLength, angle, "y"),
				(>= 315 and < 360) or (>= 0 and < 45) => GetVectorLength(xComponentLength, angle, "x"),
				_                                     => throw new ArgumentOutOfRangeException($"angle({angle}) is out of range")
			};
			
			return length;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="lineEAngle">선분의 오일러 각도</param>
		/// <param name="lineCenter">중심점</param>
		/// <param name="lineExtent">크기(size, Scale)의 절반</param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public static Vector3[] GetCornerPosArray(Vector3 lineEAngle, Vector3 lineCenter, Vector3 lineExtent)
		{
			Vector3 pos1;
			Vector3 pos2;

			switch(lineEAngle.z) {
				case >= 0 and < 90:   //제1사분면  + +
					pos1 = new Vector3(lineCenter.x + lineExtent.x, lineCenter.y + lineExtent.y, 0);
					pos2 = new Vector3(lineCenter.x - lineExtent.x, lineCenter.y - lineExtent.y, 0);
					break;
				case >= 90 and < 180: //제2사분면 - +
					pos1 = new Vector3(lineCenter.x - lineExtent.x, lineCenter.y + lineExtent.y, 0);
					pos2 = new Vector3(lineCenter.x + lineExtent.x, lineCenter.y - lineExtent.y, 0);
					break;
				case >= 180 and < 270: //제3사분면 - -
					pos1 = new Vector3(lineCenter.x - lineExtent.x, lineCenter.y - lineExtent.y, 0);
					pos2 = new Vector3(lineCenter.x + lineExtent.x, lineCenter.y + lineExtent.y, 0);
					break;
				case >= 270 and < 360: //제4사분면 + -
					pos1 = new Vector3(lineCenter.x + lineExtent.x, lineCenter.y - lineExtent.y, 0);
					pos2 = new Vector3(lineCenter.x - lineExtent.x, lineCenter.y + lineExtent.y, 0);
					break;
				default:
					throw new Exception("pos1 and pos2 are not initialized");
			}

			Vector3[] posArray = {pos2, pos1};
			return posArray;
		}

		public static Vector3 GetNearestCornerPos(GameObject vertex, GameObject edge)
		{
			var edgeEAngle = edge.transform.eulerAngles;
			var edgeCenter = edge.transform.position;
			var edgeExtent = edge.GetComponent<SpriteRenderer>().bounds.extents;
			var cornerPosArray = GetCornerPosArray(edgeEAngle, edgeCenter, edgeExtent);
			var shortestPosIndex = -1;

			var shortestDist = Infinity;

			for(var a = 0; a < cornerPosArray.Length; a++) {
				var cornerPos = cornerPosArray[a];
				var distance = Vector3.Distance(vertex.transform.position, cornerPos);

				if(!(distance < shortestDist)) continue;

				shortestDist = distance;
				shortestPosIndex = a;
			}

			return cornerPosArray[shortestPosIndex];
		}

		/*public static Vector3 GetScale(int weightedValue)
		{
			var length2Scale = Mathf.Pow(1.008f,-weightedValue) / 6;
	
			return new Vector3(weightedValue * length2Scale, 0.1f, 1f);
		}*/


		/// <summary>
		/// 특정 각(angle)을 이루고, 현재 지점(curPos)을 기준으로 radius 만큼 떨어져 있는 지점을 구하는 메소드
		/// </summary>
		/// <param name="curPos">현재 지점</param>
		/// <param name="angle">각도걊(Degree)</param>
		/// <param name="radius">반지름(curPos부터 얼마나 떨어져 있는가)</param>
		/// <returns>'curPos'을 기준으로 'radius'만큼 떨어져 있으며 'angle'도를 이루는 지점</returns>
		public static Vector3 GetPosInCircumference(Vector3 curPos, float angle, float radius)
			=> new Vector3(curPos.x + Cos(Deg2Rad * angle) * radius, curPos.y + Sin(Deg2Rad * angle) * radius);

		public static Vector3 GetRandPosInCircumference(Vector3 curPos, float radius)
		{
			var angle = (float)RandomInRange.RandomDoubleInRange(0, 360f); //angle in degree

			return new Vector3(curPos.x + Cos(Deg2Rad * angle) * radius, curPos.y + Sin(Deg2Rad * angle) * radius);
		}

		/// <summary>
		/// 오브젝트가 특정 위치를 보도록 z회전 값을 조정하는 메소드
		/// 오차가 있는 경우 보정치를 입력하여 조정한다
		/// </summary>
		/// <param name="obj">대상 오브젝트</param>
		/// <param name="posToLook">대상 오브젝트가 바라볼 위치</param>
		/// <param name="correctionVal">보정치</param>
		public static void LookAt2D(GameObject obj, Vector3 posToLook, float correctionVal = 0)
		{
			var objPos = obj.transform.position;
			var rotation = obj.transform.rotation;
			var xRot = rotation.x;
			var yRot = rotation.y;

			var deltaX = objPos.x - posToLook.x;
			var deltaY = objPos.y - posToLook.y;


			var zRot = Rad2Deg * Atan2(deltaY, deltaX) + correctionVal;
			rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
			obj.transform.rotation = rotation;
		}

		/// <summary>
		/// 시작점에서 도착점까지 일정한 속도로 이동 할 경우 걸리는 시간(초)을 구함
		/// 2차원 등속 직선 운동임을 전제한다.
		/// </summary>
		/// <param name="start">시작점 Vector3</param>
		/// <param name="dest">도착점 Vector3</param>
		/// <param name="speed">속도 (초당 위치 변화량)</param>
		/// <returns>도착점까지 걸리는 이론적 시간(초)</returns>
		public static double GetTimeElapseToDestination2D(Vector3 start, Vector3 dest, double speed)
		{
			start.z = 0;
			dest.z = 0;

			var distance = Vector3.Distance(start, dest);

			return distance / speed;
		}
		
		
		/// <summary>
		/// Gets the coordinates of the intersection point of two lines.
		/// </summary>
		/// <param name="A1">A point on the first line.</param>
		/// <param name="A2">Another point on the first line.</param>
		/// <param name="B1">A point on the second line.</param>
		/// <param name="B2">Another point on the second line.</param>
		/// <param name="found">Is set to false of there are no solution. true otherwise.</param>
		/// <see href="https://blog.dakwamine.fr/?p=1943"/>
		/// <returns>The intersection point coordinates. Returns Vector2.zero if there is no solution.</returns>
		[SuppressMessage("ReSharper", "InconsistentNaming")]
		public static Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
		{
			var tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);
 
			if (tmp == 0) { // No solution!
				found = false;
				return Vector2.zero;
			}
 
			var mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;
 
			found = true;
 
			return new Vector2(
			                   B1.x + (B2.x - B1.x) * mu,
			                   B1.y + (B2.y - B1.y) * mu
			                  );
		}

		/// <summary>
		/// 어느 위치에서 충돌했는지 왼쪽 끝 부터 충돌한 지점까지의 선분의 길이 or 퍼센트로 반환함
		/// 예들 들면, 오브젝트의 왼쪽 끝에서 충돌시 0, 오른쪽 끝에서 충돌시 100을 반환함
		/// </summary>
		/// <param name="thisCollider">충돌한 오브젝트(포탈)의 Collider2D 컴포넌트</param>
		/// <param name="thisTransform">충돌한 오브젝트(포탈)의 Transform 컴포넌트</param>
		/// <param name="contactPointVec"></param>
		/// <param name="toPercentage">반환값을 퍼센트 단위로 반환할지의 여부</param>
		/// <returns></returns>
		public static float GetLengthWhereCollidedAt(Collider2D thisCollider, Transform thisTransform, Vector3 contactPointVec, bool toPercentage)
		{
			var objColBounds = thisCollider.bounds;
			var rotation     = thisTransform.rotation.eulerAngles;
			var position     = thisTransform.position;
			var scale        = thisTransform.lossyScale;
			var vectorToMakeLeftCornerZero = -position + new Vector3(objColBounds.extents.x, objColBounds.extents.y, 0);
		
			contactPointVec += vectorToMakeLeftCornerZero;
		
			var length = GetVectorLengthInObject(contactPointVec.x, contactPointVec.y, objColBounds.size, rotation.z);
		
			// Debug.Log($"angle: {rotation.z}, length: {length}");

			return toPercentage ? length / scale.x * 100 : length;
		}

		public static float PercentLenToRealLen(Vector3 objScale, float lengthPercent)
			=> objScale.x * lengthPercent / 100;
			
		/// <summary>
		/// https://stackoverflow.com/questions/71266589/overlapped-area-between-two-colliders
		/// </summary>
		/// <param name="boundsA"></param>
		/// <param name="boundsB"></param>
		/// <returns></returns>
		public static Vector3 GetOverlappedArea(Bounds boundsA, Bounds boundsB)
		{
			// first heck whether the two objects are even overlapping at all
			if(!boundsA.Intersects(boundsB))
				return Vector3.zero;

			// now that we know they at least overlap somehow we can calculate

			// get min and max point of both
			var minA = boundsA.min; // (basically the bottom-left-back corner point)
			var maxA = boundsA.max; // (basically the top-right-front corner  point)

			var minB = boundsB.min;
			var maxB = boundsB.max;

			// we want the smaller of the max and the higher of the min points
			var lowerMax  = Vector3.Min(maxA, maxB);
			var higherMin = Vector3.Max(minA, minB);

			// the delta between those is now your overlapping area
			return lowerMax - higherMin;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="boundsA"></param>
		/// <param name="boundsB"></param>
		/// <param name="target">boundsA와 boundsB중 어느 것의 여부를 반환할 것인지 정한다. lhs/rhs만 매개변수로 받는다.</param>
		/// <returns></returns>
		public static bool GetIsFullyOverlapped(Bounds boundsA, Bounds boundsB, string target)
		{
			var overlapVec = GetOverlappedArea(boundsA, boundsB);
			return false;
		}
	
		/*
		/// <summary>
		/// Code generated by GPT-4. <br/> <br/>
		///
		/// GPT-4's comment: <br/>
		/// This code assumes that you have two colliders that you want to check for overlap. In the Start() function, it uses the Physics.OverlapBox() method to find all colliders that overlap with collider1. It then iterates through the list of overlapping colliders and checks if collider2 is among them. If it is, it uses the Intersect() method of collider2.bounds to calculate the size of the overlap with collider1.bounds. Finally, it prints the size of the overlap to the console.
		/// <br/>Note that this code assumes that the two colliders are non-convex and non-concave, as Physics.OverlapBox() only works with box-shaped colliders. If your colliders are more complex, you may need to use a different method to check for overlap.
		/// </summary>
		/// <param name="boxColA"></param>
		/// <param name="boxColB"></param>
		/// <returns></returns>
		public static Vector3 GetOverlappedArea(BoxCollider2D boxColA, BoxCollider boxColB)
		{
			// Get the overlapped area between the two colliders
			var overlappingColliders = Physics.OverlapBox(boxColA.bounds.center, boxColA.bounds.extents, Quaternion.identity, ~0, QueryTriggerInteraction.Ignore);
			var overlapSize = Vector3.zero;
			foreach (Collider col in overlappingColliders)
			{
				if (col == boxColB)
				{
					overlapSize = col.bounds.Intersect(boxColA.bounds).size;
					break;
				}
			}
			
			// Print the size of the overlap
			Debug.Log("Overlap size: " + overlapSize);
			
			return overlapSize;
		}
	*/
	}
}