using BattleArenaServer.Skills.PlagueDoctorSkills;

namespace BattleArenaServer.Models.Heroes
{
    public class PlagueDoctorHero : Hero
    {
        public PlagueDoctorHero(int Id, string Team) : base(Id, Team)
        {
            Name = "Plague Doctor";

            Respawn();

            SkillList[0] = new MicrobialSamplesPSkill(this);
            SkillList[1] = new PlagueSkill();
            SkillList[2] = new VaccineSkill();
            SkillList[3] = new TuberculosisSkill();
            SkillList[4] = new AcidSplashSkill();
        }

        public override void Respawn()
        {
            MaxHP = HP = 850;
            Armor = 2;
            Resist = 4;

            AP = 4;

            AttackRadius = 2;
            Dmg = 100;

            base.Respawn();
        }
    }
}
