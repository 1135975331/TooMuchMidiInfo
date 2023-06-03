using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Mathf;
using static Utilities.Util;
using static Utilities.VectorUtil;

namespace Utilities
{
	public static class GameObjectUtil
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="centerObj"></param>
		/// <param name="objToMove"></param>
		/// <param name="angleFrom"></param>
		/// <param name="angleTo"></param>
		/// <param name="radius"></param>
		/// <param name="duration"></param>
		public static IEnumerator MoveObjectInACircle(GameObject centerObj, GameObject objToMove, float angleFrom, float angleTo, float duration, float radius = 50)
		{
			var objToMoveCurPos = objToMove.transform;
			var curAngle        = angleFrom;
			var genDistance     = radius * 2;

			const float INTERVAL  = 0.1f;
			var         angleIncr = Abs(angleTo - angleFrom) / duration * INTERVAL;

			var waitForAMoment = new WaitForSeconds(INTERVAL);


			while(true) {
				var centerObjPos = centerObj.transform.position;
				var nextPathPos  = GetPosInCircumference(centerObjPos, curAngle, genDistance);

				var zRot                 = GetZRotation2D(centerObjPos, nextPathPos);
				var vectorForTranslation = GetVectorComponents2D(zRot.z, genDistance / 2);
				objToMove.transform.position = centerObjPos + vectorForTranslation;

				curAngle += angleIncr;

				if(curAngle >= angleTo)
					break;

				yield return waitForAMoment;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="criteriaAngle"></param>
		/// <param name="initPathAngle"></param>
		/// <param name="curMinAngle"></param>
		/// <returns></returns>
		private static float GetFinalAngleCCW(float criteriaAngle, float initPathAngle, float curMinAngle)
			=> initPathAngle + (180f + criteriaAngle - initPathAngle) * (curMinAngle - criteriaAngle) / 180f;

		private static float GetFinalAngleCW(float criteriaAngle, float initPathAngle, float curMinAngle)
			=> initPathAngle + (180f - criteriaAngle + initPathAngle) * (curMinAngle - criteriaAngle) / 180f;


		public static GameObject FindGenerator(int genNum)
		{
			return GameObject.FindGameObjectsWithTag("Generator")
			   .FirstOrDefault(genObj => genObj.name.Contains(genNum.ToString()));
		}

		/*/// <summary>
		/// 캐시 리스트에서 오브젝트를 찾아 반환하는 메소드
		/// 캐시 리스트에서 오브젝트를 찾지 못한 경우, 오브젝트를 다른 방법으로 찾아 리스트에 넣어 캐싱한 뒤에 반환한다.
		/// </summary>
		/// <param name="argToFind">오브젝트를 찾기 위한 값</param>
		/// <param name="cacheList">캐시 리스트</param>
		/// <param name="predicateType"></param>
		/// <param name="otherPredicate"></param>
		/// <param name="findFuncIfNotInCache">캐시 리스트에서 오브젝트를 찾지 못한 경우, 오브젝트를 찾기 위한 함수</param>
		/// <typeparam name="TFind">오브젝트를 찾기 위한 값의 타입</typeparam>
		/// <typeparam name="TCache">캐싱 오브젝트, 반환될 오브젝트 타입</typeparam>
		/// <returns>찾고자 하는 오브젝트</returns>
		public static TCache FindObjInCache<TFind, TCache>(TFind argToFind, List<TCache> cacheList, string predicateType, Func<TFind, bool> otherPredicate, Func<TFind, TCache> findFuncIfNotInCache) where TCache : UnityEngine.Object
		{
			var predicateFindInCacheList = predicateType.ToLower() switch {
				"name_equal"   => elem => elem.ToString().Equals(argToFind),
				"name_contain" => elem => elem.ToString().Contains(argToFind.ToString()),
				"other"        => otherPredicate,
					_          => throw new ArgumentOutOfRangeException(nameof(predicateType), predicateType, null)
			};

			
			var filteredCacheList = cacheList.Where(predicateFindInCacheList).ToList();
			if(filteredCacheList.Any())  // 값이 하나라도 들어있는 경우
				return filteredCacheList.First(); // InvalidOperationException이 발생한 경우, filteredCachedList에 아무런 값이 없음을 의미함

			// cacheList 리스트에서 찾지 못했으면 Find명령어로 찾은 뒤 캐싱하고 returnList에 추가한다.
			var objNotCached = findFuncIfNotInCache(argToFind);
			cacheList.Add(objNotCached);
			return objNotCached;
		}*/

		/// <summary>
		/// 캐시 리스트에서 오브젝트를 찾아 반환하는 메소드
		/// 캐시 리스트에서 오브젝트를 찾지 못한 경우, 오브젝트를 다른 방법으로 찾아 리스트에 넣어 캐싱한 뒤에 반환한다.
		/// </summary>
		/// <param name="argToFind">오브젝트를 찾기 위한 값</param>
		/// <param name="cacheSet">캐시 리스트</param>
		/// <param name="predicateType">name_equal(name.Equals()), name_contain(name.Contains())</param>
		/// <typeparam name="TCache">캐싱 오브젝트, 반환될 오브젝트 타입</typeparam>
		/// <returns>찾고자 하는 오브젝트</returns>
		public static TCache FindObjInCache<TCache>(string argToFind, HashSet<TCache> cacheSet, string predicateType, Func<HashSet<TCache>> findAllTCacheObj) where TCache : UnityEngine.Object
		{
			Func<TCache, bool> predicate = predicateType switch {
				"name_equal"   => elem => elem.name.Equals(argToFind),
				"name_contain" => elem => elem.name.Contains(argToFind),
				_            => throw new ArgumentOutOfRangeException(nameof(predicateType), predicateType, null)
			};
			
			var filteredCacheList = cacheSet.Where(predicate).ToHashSet();
			if(filteredCacheList.Any())           // 값이 하나라도 들어있는 경우
				return filteredCacheList.First(); // InvalidOperationException이 발생한 경우, filteredCachedList에 아무런 값이 없음을 의미함

			// cacheList 리스트에서 찾지 못했으면 Find명령어로 찾은 뒤 캐싱하고 returnList에 추가한다.
			var allTCacheObj = findAllTCacheObj();
			
			foreach(var obj in allTCacheObj)
				cacheSet.Add(obj);
			
			return cacheSet.Where(predicate).ToHashSet().First();  // 여기서도 InvalidOperationException이 일어났다면, 찾으려는 오브젝트 자체가 존재하지 않음을 의미할 수도 있으므로 채보파일을 확인할 것.
		}
	}
}