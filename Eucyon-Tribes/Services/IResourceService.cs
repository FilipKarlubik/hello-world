using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;

namespace Eucyon_Tribes.Services
{
    public interface IResourceService
    {
        void UpdateResource();

        void UpdateWoodResource(Kingdom kingdom, DateTime updateTime);

        void UpdateGoldResource(Kingdom kingdom, DateTime updateTime);

        void UpdatePeopleResource(Kingdom kingdom, DateTime updateTime);

        void UpdateFoodResource(Kingdom kingdom, DateTime updateTime);
        public ResourcesDTO UpdateResourceKingdom(int id);

        public void OpenConnection();

        public void FamineCheck();
    }
}
