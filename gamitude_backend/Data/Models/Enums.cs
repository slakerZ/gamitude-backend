namespace gamitude_backend.Models
{
    public enum STATS { STRENGTH, INTELLIGENCE, CREATIVITY, FLUENCY, BODY, EMOTIONS, MIND, SOUL };
    public enum PROJECT_TYPE { STAT, ENERGY, BREAK };
    public enum TIMER_TYPE { STOPWATCH, TIMER };
    public enum PAGE_TYPE { NORMAL, OVERDUE, UNSCHEDULED }
    public enum CURRENCY { REAL, STATS };
    public enum SORT_TYPE { ASC, DESC };
    public enum GAMITUDE_STYLE
    {
        DEFAULT, WINTER, BUSINESS, LOL
    }
    public enum RANK_TIER
    {
        F, D, C, B, A, S
    }
    public enum RANK_DOMINANT
    {
        STRENGHT, INTELLIGENCE, FLUENCY, CREATIVITY, BALANCED

    }
}