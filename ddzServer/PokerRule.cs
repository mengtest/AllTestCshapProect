using System.Collections.Generic;

namespace ddzServer
{
    public class PokerRule
    {
        public PokerValue min = PokerValue._3;
        public PokerValue max = PokerValue._3;
        public int num = 0;
        public int xushu = 0;
        public int lzNum = 0;
        public List<PokerRule> _rule = new List<PokerRule>();
        Dictionary<int,List<PokerRule>> dic = new Dictionary<int, List<PokerRule>>(){
            {1,new List<PokerRule>(){
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue.laizi,num = 1,xushu = 0,lzNum = 0
                },
            }},
            {2,new List<PokerRule>(){
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue.maxJoker,num = 2,xushu = 0,lzNum = 1
                },
            }},
            {3,new List<PokerRule>(){
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue._2,num = 3,xushu = 0,lzNum = 2
                },
            }},
            {4,new List<PokerRule>(){
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue._2,num = 4,xushu = 0,lzNum = 3
                },
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue._2,num = 3,xushu = 0,lzNum = 2,_rule = new List<PokerRule>(){
                        new PokerRule(){
                                min = PokerValue._3,max = PokerValue.maxJoker,num = 1,xushu = 0,lzNum = 0
                        },
                    }
                },
            }},
            {5,new List<PokerRule>(){
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue.A,num = 1,xushu = 5,lzNum = 4
                },
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue._2,num = 3,xushu = 0,lzNum = 2,_rule = new List<PokerRule>(){
                        new PokerRule(){
                                min = PokerValue._3,max = PokerValue.maxJoker,num = 2,xushu = 0,lzNum = 1
                        },
                    }
                },
            }},
            {6,new List<PokerRule>(){
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue.A,num = 2,xushu = 3,lzNum = 3
                },
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue.A,num = 1,xushu = 6,lzNum = 4
                },
                new PokerRule(){
                    min = PokerValue._3,max = PokerValue._2,num = 4,xushu = 0,lzNum = 3,_rule = new List<PokerRule>(){
                        new PokerRule(){
                                min = PokerValue._3,max = PokerValue.maxJoker,num = 1,xushu = 0,lzNum = 0
                        },
                        new PokerRule(){
                                min = PokerValue._3,max = PokerValue.maxJoker,num = 1,xushu = 0,lzNum = 0
                        },
                    }
                },
            }},
        };
    }
}