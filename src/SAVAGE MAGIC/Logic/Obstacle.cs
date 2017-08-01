using Newtonsoft.Json.Linq;
using System;
using Magic.ClashOfClans.Core;
using Magic.ClashOfClans;
using Magic.Files.Logic;

namespace Magic.ClashOfClans.Logic
{
	internal class Obstacle : GameObject
	{
		private readonly Level m_vLevel;

		private Timer m_vTimer;

		public Obstacle(Data data, Level l) : base(data, l)
		{
			m_vLevel = l;
		}

		public override int ClassId
		{
			get { return 3; }
		}

		public void CancelClearing()
		{
			m_vLevel.WorkerManager.DeallocateWorker(this);
			m_vTimer = null;
			var od = GetObstacleData();
			var rd = od.GetClearingResource();
			var cost = od.ClearCost;
			Level.Avatar.CommodityCountChangeHelper(0, rd, cost);
		}

		public void ClearingFinished()
		{
			//m_vLevel.GameObjectManager.GetObstacleManager().IncreaseObstacleClearCount();
			m_vLevel.WorkerManager.DeallocateWorker(this);
			m_vTimer = null;
			var constructionTime = GetObstacleData().ClearTimeSeconds;
            var exp = (int)Math.Sqrt(constructionTime);

            Level.Avatar.AddExperience(exp);

			var rd = CsvManager.DataTables.GetResourceByName(GetObstacleData().LootResource);

			Level.Avatar.CommodityCountChangeHelper(0, rd, GetObstacleData().LootCount);

			Level.GameObjectManager.RemoveGameObject(this);
		}

		public ObstacleData GetObstacleData()
		{
			return (ObstacleData)Data;
		}

		public int GetRemainingClearingTime()
		{
			return m_vTimer.GetRemainingSeconds(m_vLevel.Time);
		}

		public bool IsClearingOnGoing()
		{
			return m_vTimer != null;
		}

		public void SpeedUpClearing()
		{
			var remainingSeconds = 0;
			if (IsClearingOnGoing())
			{
				remainingSeconds = m_vTimer.GetRemainingSeconds(m_vLevel.Time);
			}
			var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
			var ca = Level.Avatar;
			if (ca.HasEnoughDiamonds(cost))
			{
				ca.UseDiamonds(cost);
				ClearingFinished();
			}
		}

		public void StartClearing()
		{
			var constructionTime = GetObstacleData().ClearTimeSeconds;
			if (constructionTime < 1)
			{
				ClearingFinished();
			}
			else
			{
				m_vTimer = new Timer();
				m_vTimer.StartTimer(constructionTime, m_vLevel.Time);
				m_vLevel.WorkerManager.AllocateWorker(this);
			}
		}

		public override void Tick()
		{
			if (IsClearingOnGoing())
			{
				if (m_vTimer.GetRemainingSeconds(m_vLevel.Time) <= 0)
					ClearingFinished();
			}
		}

		public JObject ToJson()
		{
			var jsonObject = new JObject();
			jsonObject.Add("data", GetObstacleData().GetGlobalID());
			if (IsClearingOnGoing())
				jsonObject.Add("const_t", m_vTimer.GetRemainingSeconds(m_vLevel.Time));
			jsonObject.Add("x", X);
			jsonObject.Add("y", Y);
			return jsonObject;
		}
	}
}
